using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;
using omech.Models;

namespace omech.Services
{
    public interface IAiChatService
    {
        Task<AiChatResponse> AskAsync(AiChatRequest request);
    }

    public class AiChatService : IAiChatService
    {
        private const int MaxToolLoops = 8;

        // MVP session store: per-process, in-memory, keyed by SessionId.
        // Move to Redis/DB before production so history survives a restart
        // and works across multiple backend instances.
        private static readonly ConcurrentDictionary<string, JsonArray> _sessions = new();

        private readonly AnthropicClient _anthropic;
        private readonly AiToolCatalog _tools;

        public AiChatService(AnthropicClient anthropic, AiToolCatalog tools)
        {
            _anthropic = anthropic;
            _tools = tools;
        }

        public async Task<AiChatResponse> AskAsync(AiChatRequest request)
        {
            var comPara = new ComParaModel { USER_SRNO = request.USER_SRNO, UT_SRNO = request.UT_SRNO };
            var history = _sessions.GetOrAdd(request.SessionId, _ => new JsonArray());

            history.Add(new JsonObject { ["role"] = "user", ["content"] = request.Message });

            var toolDefs = _tools.GetToolDefinitions();

            // Every data tool's rows this turn, keyed by tool name, so the
            // final report can reference them for tables and charts.
            var collectedData = new Dictionary<string, JsonArray>();

            for (var i = 0; i < MaxToolLoops; i++)
            {
                var response = await _anthropic.SendAsync(BuildSystemPrompt(), CloneArray(history), toolDefs);
                var contentBlocks = response["content"]!.AsArray();
                var stopReason = response["stop_reason"]?.GetValue<string>();

                history.Add(new JsonObject { ["role"] = "assistant", ["content"] = CloneArray(contentBlocks) });

                if (stopReason != "tool_use")
                {
                    // Plain conversational reply — no data was needed.
                    return new AiChatResponse { Summary = ExtractText(contentBlocks) };
                }

                var toolResults = new JsonArray();
                AiChatResponse? finalReport = null;

                foreach (var block in contentBlocks)
                {
                    if (block?["type"]?.GetValue<string>() != "tool_use") continue;

                    var toolName = block["name"]!.GetValue<string>();
                    var toolUseId = block["id"]!.GetValue<string>();
                    var input = block["input"];

                    if (toolName == "present_report")
                    {
                        finalReport = BuildReport(input, collectedData);
                        toolResults.Add(new JsonObject
                        {
                            ["type"] = "tool_result",
                            ["tool_use_id"] = toolUseId,
                            ["content"] = "Report delivered to user."
                        });
                        continue;
                    }

                    string resultJson;
                    try
                    {
                        resultJson = _tools.Execute(toolName, input?.DeepClone(), comPara);
                        var parsed = JsonNode.Parse(resultJson);
                        var rows = ExtractFirstTable(parsed?["data"]);
                        if (rows != null) collectedData[toolName] = rows;
                    }
                    catch (Exception ex)
                    {
                        resultJson = JsonSerializer.Serialize(new { msgId = 500, msg = $"Tool error: {ex.Message}" });
                    }

                    toolResults.Add(new JsonObject
                    {
                        ["type"] = "tool_result",
                        ["tool_use_id"] = toolUseId,
                        ["content"] = resultJson
                    });
                }

                history.Add(new JsonObject { ["role"] = "user", ["content"] = toolResults });

                if (finalReport != null) return finalReport;
            }

            return new AiChatResponse
            {
                Summary = "I wasn't able to finish gathering that data in a reasonable number of steps — try narrowing the question a bit."
            };
        }

        private static AiChatResponse BuildReport(JsonNode? input, Dictionary<string, JsonArray> collectedData)
        {
            var report = new AiChatResponse
            {
                Summary = input?["summary"]?.GetValue<string>() ?? "",
                Analysis = input?["analysis"]?.GetValue<string>() ?? ""
            };

            if (input?["key_metrics"] is JsonArray metricSpecs)
            {
                foreach (var m in metricSpecs)
                {
                    report.KeyMetrics.Add(new AiChatMetric
                    {
                        Label = m?["label"]?.GetValue<string>() ?? "",
                        Value = m?["value"]?.GetValue<string>() ?? ""
                    });
                }
            }

            // Every dataset gathered this turn becomes a table in the report.
            foreach (var (toolName, rows) in collectedData)
            {
                report.Tables.Add(new AiChatTable
                {
                    Title = FriendlyName(toolName),
                    Rows = rows.Select(RowToDict).ToList()
                });
            }

            // Charts reference one of those datasets by tool name.
            if (input?["charts"] is JsonArray chartSpecs)
            {
                foreach (var spec in chartSpecs)
                {
                    var sourceTool = spec?["source_tool"]?.GetValue<string>();
                    if (sourceTool == null || !collectedData.TryGetValue(sourceTool, out var rows)) continue;

                    report.Charts.Add(new AiChatChart
                    {
                        Title = spec!["title"]?.GetValue<string>() ?? "",
                        Type = spec["type"]?.GetValue<string>() ?? "bar",
                        XField = spec["x_field"]?.GetValue<string>() ?? "",
                        YField = spec["y_field"]?.GetValue<string>() ?? "",
                        GroupByField = spec["group_by_field"]?.GetValue<string>(),
                        Data = rows.Select(RowToDict).ToList()
                    });
                }
            }

            return report;
        }

        private static Dictionary<string, object?> RowToDict(JsonNode? row)
        {
            var dict = new Dictionary<string, object?>();
            if (row is JsonObject obj)
            {
                foreach (var (key, value) in obj)
                {
                    dict[key] = value is null ? null : JsonSerializer.Deserialize<object>(value.ToJsonString());
                }
            }
            return dict;
        }

        // Your DataService responses look like { msgId, msg, data: { "Table": [ ...rows ] } }.
        // Pull out the first table's row array.
        private static JsonArray? ExtractFirstTable(JsonNode? dataNode)
        {
            if (dataNode is not JsonObject dataObj) return null;
            foreach (var (_, value) in dataObj)
            {
                if (value is JsonArray arr) return arr;
            }
            return null;
        }

        private static string FriendlyName(string toolName) => toolName switch
        {
            "get_raw_material" => "Raw material",
            "get_raw_inventory_status" => "Raw material inventory",
            "get_pipe_inventory" => "Pipe inventory",
            "get_pipe_inventory_movements" => "Inventory movements",
            "get_order_schedule_analysis" => "Order / schedule analysis",
            "get_low_stock_alerts" => "Low stock alerts",
            _ => toolName
        };

        private static string ExtractText(JsonArray contentBlocks) =>
            string.Join("\n", contentBlocks
                .Where(b => b?["type"]?.GetValue<string>() == "text")
                .Select(b => b!["text"]!.GetValue<string>()));

        private static JsonArray CloneArray(JsonArray source) => (JsonArray)JsonNode.Parse(source.ToJsonString())!;

        private static string BuildSystemPrompt() => $@"
You are a reporting and analysis assistant for OMECH, a pipe manufacturing
company, embedded in their internal inventory management dashboard. Staff
ask questions in plain language about raw materials, production, finished-
goods inventory, stock movements, customer orders, and low-stock alerts.
Your output is read directly by managers and shown to clients as a
polished report, so treat every report like a business analyst would.

Today's date is {DateTime.Now:yyyy-MM-dd} ({DateTime.Now:MMMM yyyy}). Always
use this — not any date from your training data — to resolve relative
periods like 'last month', 'this year', 'last week', 'yesterday', etc.

Rules:
- Only answer using data returned by your tools. Never guess or make up numbers.
- If the user refers to a grade, OD, thickness, or location by name (not by
  numeric id), call get_lookup_lists first to resolve the name to its id,
  then call the relevant data tool with that id.
- BROAD QUESTIONS GET FULL COVERAGE. If the question is general — e.g. 'what
  is my current inventory', 'give me a report on our stock', 'how are we
  doing' — call EVERY tool that's relevant to build a complete picture, not
  just one. 'Current inventory' means BOTH get_raw_inventory_status AND
  get_pipe_inventory, for example. Do not stop after the first tool call if
  more of the question is still unanswered.
- If a question is ambiguous (e.g. no date range given), make a reasonable
  assumption (e.g. current month, based on today's real date above) and say
  what you assumed in the analysis.
- Once you've gathered all the data you need, call present_report exactly
  ONCE as your final step. Do not write a plain text final answer for data
  questions — always finish through present_report.
- Only skip present_report for purely conversational messages that need no
  data at all (greetings, thanks, clarifying questions).
- ALWAYS include 2-5 key_metrics for any report covering more than a
  trivial single lookup — these are the headline numbers a manager would
  want at a glance (totals, counts, sums), formatted with units and
  thousands separators.
- Write a real analysis, not a restatement of the numbers. Call out the
  biggest category, anything unusually low or high, notable comparisons
  between groups, or things that need attention (e.g. low stock, an
  outlier). Aim for 2-5 sentences of genuine insight, like a human analyst
  would give, not just 'X units of Y were found.'
- DEFAULT TO INCLUDING A CHART for anything with more than 2-3 data points
  to compare (by grade, by OD, by month, by status, etc.) — a chart makes
  the report easier to read at a glance. Only skip it for a true single-
  number answer with nothing to compare.
- For follow-up questions referring to 'that' or 'the same data', use the
  conversation history rather than re-asking the user.
";
    }
}

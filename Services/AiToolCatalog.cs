using System.Text.Json;
using System.Text.Json.Nodes;
using omech.Models;

namespace omech.Services
{
    /// <summary>
    /// Defines the tools Claude is allowed to call, and executes them against
    /// the existing IDataService methods (same stored procedures your
    /// dashboards already use). No new SQL is written here — every tool is a
    /// thin, read-only wrapper around a method that's already in production.
    /// </summary>
    public class AiToolCatalog
    {
        private readonly IDataService _dataService;

        public AiToolCatalog(IDataService dataService)
        {
            _dataService = dataService;
        }

        // ---- Tool definitions sent to Claude on every request ----
        public JsonArray GetToolDefinitions()
        {
            return new JsonArray
            {
                Tool(
                    "get_lookup_lists",
                    "Get the id lists for Grade, OD, Thickness, Location, and Machine. ALWAYS call this first before any other tool if the user refers to a grade, OD, thickness, location, or machine by name (e.g. 'grade X', 'OD 1.2', 'TM 3') rather than by its numeric id — every other tool needs the id, not the name.",
                    Props()
                ),
                Tool(
                    "get_raw_material",
                    "Raw material coils received from suppliers. Use for questions about incoming coils, challans, weight received, or scrap at the raw material stage.",
                    Props(
                        StrProp("challan_no", "Filter by challan number"),
                        DateProp("date_from", "Start of receive-date range (YYYY-MM-DD)"),
                        DateProp("date_to", "End of receive-date range (YYYY-MM-DD)"),
                        StrProp("supplier", "Supplier/vendor name"),
                        IntProp("grade_srno", "Grade lookup id — resolve the grade name to its id first if you don't already know it"),
                        IntProp("thickness_srno", "Thickness lookup id")
                    )
                ),
                Tool(
                    "get_raw_inventory_status",
                    "Current raw material inventory / stock status (coils available, in slitting, etc). Use for 'how much raw material do we have' type questions.",
                    Props(
                        DateProp("date_from", "Start date"),
                        DateProp("date_to", "End date"),
                        IntProp("grade_srno", "Grade lookup id"),
                        IntProp("thickness_srno", "Thickness lookup id"),
                        NumProp("width", "Filter by material width"),
                        IntProp("status_srno", "Status lookup id (e.g. New, In Slitting, Sold, Returned, Slitted, Completed)"),
                        IntProp("location_srno", "Vendor/location id (C_LOCATION)")
                    )
                ),
                Tool(
                    "get_pipe_inventory",
                    "Current finished-goods pipe inventory (stock on hand), by product spec. Use for 'how much stock of grade X, OD 1.2 do we have' type questions.",
                    Props(
                        IntProp("grade_srno", "Grade lookup id"),
                        IntProp("thickness_srno", "Thickness lookup id"),
                        IntProp("od_srno", "OD lookup id"),
                        IntProp("location_srno", "Location id"),
                        NumProp("pr_length", "Pipe length")
                    )
                ),
                Tool(
                    "get_pipe_inventory_movements",
                    "Ledger of finished-goods stock movements (production added, sold, shifted, returned, cut) over a date range. Use for 'how much was produced/sold last month', or 'what did machine X produce' type questions.",
                    Props(
                        IntProp("grade_srno", "Grade lookup id"),
                        IntProp("thickness_srno", "Thickness lookup id"),
                        IntProp("od_srno", "OD lookup id"),
                        IntProp("location_srno", "Location id"),
                        NumProp("pr_length", "Pipe length"),
                        IntProp("inv_type", "Transaction type id (add/shift/sell/return/cut — see M_PIPE_TRN_TYPE)"),
                        IntProp("machine_srno", "Machine lookup id — call get_lookup_lists first if the user names a machine (e.g. 'TM 3') rather than giving its id"),
                        DateProp("date_from", "Start date"),
                        DateProp("date_to", "End date")
                    )
                ),
                Tool(
                    "get_order_schedule_analysis",
                    "Customer PO / schedule analysis: ordered vs dispatched vs pending quantities. Use for order fulfilment, customer, and delivery-date questions.",
                    Props(
                        IntProp("grade_srno", "Grade lookup id"),
                        IntProp("thickness_srno", "Thickness lookup id"),
                        IntProp("od_srno", "OD lookup id"),
                        StrProp("party_name", "Customer/party name"),
                        StrProp("po_number", "PO number"),
                        DateProp("entry_date_from", "Order entry date range start"),
                        DateProp("entry_date_to", "Order entry date range end"),
                        DateProp("delivery_date_from", "Delivery date range start"),
                        DateProp("delivery_date_to", "Delivery date range end")
                    )
                ),
                Tool(
                    "get_low_stock_alerts",
                    "Products currently below their configured minimum stock threshold. Use for 'what's running low' / 'what should we reorder' type questions. Call with no filters to see everything below threshold.",
                    Props(
                        IntProp("grade_srno", "Grade lookup id"),
                        IntProp("thickness_srno", "Thickness lookup id"),
                        IntProp("od_srno", "OD lookup id")
                    )
                ),
                new JsonObject
                {
                    ["name"] = "present_report",
                    ["description"] = "Call this ONCE as your final step for any question that needs data, a number, or analysis \u2014 after you've gathered everything you need with the other tools. Do not call it for purely conversational messages (greetings, clarifying questions) that need no data.",
                    ["input_schema"] = new JsonObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JsonObject
                        {
                            ["summary"] = new JsonObject { ["type"] = "string", ["description"] = "1-2 sentence headline answer" },
                            ["key_metrics"] = new JsonObject
                            {
                                ["type"] = "array",
                                ["description"] = "2-5 headline KPI numbers for an at-a-glance view, e.g. total stock, total weight, item count. Always include these for any report covering more than a trivial single lookup.",
                                ["items"] = new JsonObject
                                {
                                    ["type"] = "object",
                                    ["properties"] = new JsonObject
                                    {
                                        ["label"] = new JsonObject { ["type"] = "string", ["description"] = "Short label, e.g. 'Total Raw Material Stock'" },
                                        ["value"] = new JsonObject { ["type"] = "string", ["description"] = "Formatted value with unit, e.g. '12,450 kg' or '8,200 pcs'" }
                                    },
                                    ["required"] = new JsonArray { "label", "value" }
                                }
                            },
                            ["analysis"] = new JsonObject { ["type"] = "string", ["description"] = "A written analysis with real insight, not just a restatement of the numbers \u2014 call out the biggest category, anything unusually low/high, notable comparisons, or things needing attention. Plain text, no markdown headers." },
                            ["charts"] = new JsonObject
                            {
                                ["type"] = "array",
                                ["description"] = "Optional. Only include a chart when it genuinely helps \u2014 e.g. a trend over time or a comparison across categories. Skip for single-number answers.",
                                ["items"] = new JsonObject
                                {
                                    ["type"] = "object",
                                    ["properties"] = new JsonObject
                                    {
                                        ["title"] = new JsonObject { ["type"] = "string" },
                                        ["type"] = new JsonObject { ["type"] = "string", ["enum"] = new JsonArray { "bar", "line", "pie" } },
                                        ["source_tool"] = new JsonObject { ["type"] = "string", ["description"] = "Name of the data tool whose rows this chart is built from, e.g. get_pipe_inventory_movements" },
                                        ["x_field"] = new JsonObject { ["type"] = "string", ["description"] = "Column name to use as the x-axis / category / pie label" },
                                        ["y_field"] = new JsonObject { ["type"] = "string", ["description"] = "Column name to use as the numeric value" },
                                        ["group_by_field"] = new JsonObject { ["type"] = "string", ["description"] = "Optional column to split into multiple series" }
                                    },
                                    ["required"] = new JsonArray { "title", "type", "source_tool", "x_field", "y_field" }
                                }
                            }
                        },
                        ["required"] = new JsonArray { "summary" }
                    }
                }
            };
        }

        // ---- Executes a tool call and returns JSON text for Claude ----
        public string Execute(string toolName, JsonNode? input, ComParaModel comPara)
        {
            input ??= new JsonObject();

            object result = toolName switch
            {
                "get_lookup_lists" => _dataService.Pl_Common(comPara, "1,2,3,4,8"),

                "get_raw_material" => _dataService.DtRawMaterial(
                    comPara,
                    Str(input, "challan_no"),
                    Dt(input, "date_from"),
                    Dt(input, "date_to"),
                    Str(input, "supplier"),
                    IntV(input, "grade_srno"),
                    IntV(input, "thickness_srno"),
                    'Y'),

                "get_raw_inventory_status" => _dataService.DtDashRawInventory(
                    comPara,
                    'A',
                    Dt(input, "date_from"),
                    Dt(input, "date_to"),
                    IntV(input, "grade_srno"),
                    IntV(input, "thickness_srno"),
                    Dec(input, "width"),
                    IntV(input, "status_srno"),
                    IntV(input, "location_srno")),

                "get_pipe_inventory" => _dataService.DtPipes(
                    comPara,
                    null,
                    IntV(input, "grade_srno"),
                    IntV(input, "thickness_srno"),
                    IntV(input, "od_srno"),
                    IntV(input, "location_srno"),
                    IntV(input, "pr_length")),

                "get_pipe_inventory_movements" => _dataService.DtPipesLogs(
                    comPara,
                    null,
                    IntV(input, "grade_srno"),
                    IntV(input, "thickness_srno"),
                    IntV(input, "od_srno"),
                    IntV(input, "location_srno"),
                    IntV(input, "pr_length"),
                    IntV(input, "inv_type"),
                    Dt(input, "date_from"),
                    Dt(input, "date_to")),

                "get_order_schedule_analysis" => _dataService.DtScheduleAnalysis(
                    comPara,
                    IntV(input, "grade_srno"),
                    IntV(input, "thickness_srno"),
                    IntV(input, "od_srno"),
                    Str(input, "party_name"),
                    Str(input, "po_number"),
                    Dt(input, "entry_date_from"),
                    Dt(input, "entry_date_to"),
                    Dt(input, "delivery_date_from"),
                    Dt(input, "delivery_date_to")),

                "get_low_stock_alerts" => _dataService.DtThresholdDisplay(
                    comPara,
                    IntV(input, "grade_srno"),
                    IntV(input, "thickness_srno"),
                    IntV(input, "od_srno"),
                    null),

                "present_report" => new { msgId = 200, msg = "Report captured.", data = (object?)null },

                _ => new { msgId = 500, msg = $"Unknown tool: {toolName}", data = (object?)null }
            };

            return JsonSerializer.Serialize(result);
        }

        // ---- small helpers ----
        private static string? Str(JsonNode input, string key) => input[key]?.GetValue<string>();
        private static int? IntV(JsonNode input, string key) =>
            input[key] is JsonValue v && v.TryGetValue<int>(out var i) ? i : null;
        private static decimal? Dec(JsonNode input, string key) =>
            input[key] is JsonValue v && v.TryGetValue<decimal>(out var d) ? d : null;
        private static DateTime? Dt(JsonNode input, string key) =>
            input[key] is JsonValue v && v.TryGetValue<string>(out var s) && DateTime.TryParse(s, out var dt) ? dt : null;

        private static JsonObject Tool(string name, string description, JsonObject properties) => new()
        {
            ["name"] = name,
            ["description"] = description,
            ["input_schema"] = new JsonObject
            {
                ["type"] = "object",
                ["properties"] = properties
            }
        };

        private static JsonObject Props(params (string name, JsonObject schema)[] props)
        {
            var obj = new JsonObject();
            foreach (var (name, schema) in props) obj[name] = schema;
            return obj;
        }

        private static (string, JsonObject) StrProp(string name, string desc) => (name, new JsonObject { ["type"] = "string", ["description"] = desc });
        private static (string, JsonObject) IntProp(string name, string desc) => (name, new JsonObject { ["type"] = "integer", ["description"] = desc });
        private static (string, JsonObject) NumProp(string name, string desc) => (name, new JsonObject { ["type"] = "number", ["description"] = desc });
        private static (string, JsonObject) DateProp(string name, string desc) => (name, new JsonObject { ["type"] = "string", ["description"] = desc });
    }
}

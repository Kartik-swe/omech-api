using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace omech.Services
{
    public class AnthropicClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _model;

        public AnthropicClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["AnthropicSettings:ApiKey"]
                ?? throw new InvalidOperationException("AnthropicSettings:ApiKey is not configured.");
            _model = config["AnthropicSettings:Model"] ?? "claude-haiku-4-5";

            _http.BaseAddress = new Uri("https://api.anthropic.com/v1/");
            _http.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            _http.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        }

        /// <summary>
        /// Sends the conversation so far (+ tool defs) to Claude and returns
        /// the raw parsed response body as a JsonNode.
        /// </summary>
        public async Task<JsonNode> SendAsync(string systemPrompt, JsonArray messages, JsonArray tools)
        {
            // Defensive clone: a JsonNode can only belong to one parent at a
            // time. Both `messages` and `tools` may get reused across
            // multiple calls within a single tool-use loop, so attaching the
            // same instance twice throws "The node already has a parent".
            var body = new JsonObject
            {
                ["model"] = _model,
                ["max_tokens"] = 2000,
                ["system"] = systemPrompt,
                ["messages"] = JsonNode.Parse(messages.ToJsonString()),
                ["tools"] = JsonNode.Parse(tools.ToJsonString())
            };

            var content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("messages", content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Anthropic API error ({(int)response.StatusCode}): {responseText}");
            }

            return JsonNode.Parse(responseText)!;
        }
    }
}

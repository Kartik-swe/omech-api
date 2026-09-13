using System.Collections.Generic;

namespace omech.Models
{
    public class AiChatRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int USER_SRNO { get; set; }
        public int UT_SRNO { get; set; }
    }

    public class AiChatChart
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = "bar"; // bar | line | pie
        public string XField { get; set; } = string.Empty;
        public string YField { get; set; } = string.Empty;
        public string? GroupByField { get; set; }
        public List<Dictionary<string, object?>> Data { get; set; } = new();
    }

    public class AiChatTable
    {
        public string Title { get; set; } = string.Empty;
        public List<Dictionary<string, object?>> Rows { get; set; } = new();
    }

    public class AiChatMetric
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class AiChatResponse
    {
        // Always present: the headline answer, one or two sentences.
        public string Summary { get; set; } = string.Empty;

        // KPI-style numbers for an at-a-glance view.
        public List<AiChatMetric> KeyMetrics { get; set; } = new();

        // Present for report-style answers: a short written analysis.
        // Empty for simple conversational replies (e.g. greetings, clarifications).
        public string Analysis { get; set; } = string.Empty;

        public List<AiChatTable> Tables { get; set; } = new();
        public List<AiChatChart> Charts { get; set; } = new();
    }
}

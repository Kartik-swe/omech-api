using System.Collections.Generic;

namespace omech.Models
{
    /// <summary>
    /// Request model used to talk to the AI chat service.
    /// </summary>
    public class AiChatRequest
    {
        /// <summary>Conversation/session identifier to correlate messages.</summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>User prompt or question for the AI service.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>Numeric user id (optional)</summary>
        public int USER_SRNO { get; set; }

        /// <summary>Numeric user type id (optional)</summary>
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
        /// <summary>Headline answer or summary text.</summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>KPI-style key metrics to display.</summary>
        public List<AiChatMetric> KeyMetrics { get; set; } = new();

        /// <summary>Long-form analysis when applicable.</summary>
        public string Analysis { get; set; } = string.Empty;

        /// <summary>Optional tables returned by the AI assistant.</summary>
        public List<AiChatTable> Tables { get; set; } = new();

        /// <summary>Optional charts returned by the AI assistant.</summary>
        public List<AiChatChart> Charts { get; set; } = new();
    }
}

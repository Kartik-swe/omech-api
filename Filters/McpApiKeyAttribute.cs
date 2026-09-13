using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace omech.Filters
{
    /// <summary>
    /// Simple shared-secret check for the /api/Mcp endpoints. These are
    /// called by the MCP server (a backend process, not a logged-in OMECH
    /// user), so they don't use the normal JWT [Authorize] flow — instead
    /// they require a fixed API key header that only your MCP server knows.
    /// </summary>
    public class McpApiKeyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var expectedKey = config["McpSettings:ApiKey"];

            if (string.IsNullOrEmpty(expectedKey) ||
                !context.HttpContext.Request.Headers.TryGetValue("X-Mcp-Api-Key", out var providedKey) ||
                providedKey != expectedKey)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult(
                    new { msgId = 401, msg = "Invalid or missing MCP API key." });
            }
        }
    }
}

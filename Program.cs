using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using omech.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Added New code
// Configure CORS (Cross-Origin Resource Sharing)
    builder.Services.AddCors(options =>
    {
        // Define a CORS policy
        options.AddPolicy("AllowAllOrigins", builder =>
            builder.AllowAnyOrigin()  // Allow requests from any origin 
                   .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
                   .AllowAnyHeader());  // Allow any header
    });

// Add services to the container.
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("db_dev_con");

builder.Services.AddSingleton(new DatabaseHelper(connectionString));
builder.Services.AddControllers();



// Register the DataService as a scoped service (if it needs to be scoped)
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddHttpClient<AnthropicClient>();
builder.Services.AddScoped<AiToolCatalog>();
builder.Services.AddScoped<IAiChatService, AiChatService>();

// Added JWT Authentication
var jwtSettings = configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ---------------------------
// 🔐 Add IP restriction here
// ---------------------------
//var allowedIps = new HashSet<string>
//{
//    "103.197.224.12",
//    "61.0.43.246",
//    "152.56.0.183",
//    "152.56.5.0"
//};
// Get Allowed IPs from configuration
var allowedIps = configuration.GetSection("AllowedIPs").Get<List<string>>() ?? new List<string>();

app.Use(async (context, next) =>
{
    // /api/Mcp has its own API-key auth (see McpApiKeyAttribute) and is
    // called by the MCP server container, not a browser — skip the IP
    // allowlist for this path only.
    if (context.Request.Path.StartsWithSegments("/api/Mcp"))
    {
        await next();
        return;
    }

    // Get IP from proxy header (real client IP)
    var forwardedIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

    // Fallback to connection IP
    var ip = forwardedIp ?? context.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    //var ip = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();

    if (ip != null && allowedIps.Contains(ip))
    {
        await next();
    }
    else
    {
        //await next();

         //context.Response.StatusCode = StatusCodes.Status403Forbidden;
        //await context.Response.WriteAsync("Access denied: Your IP is not allowed.");
        await context.Response.WriteAsync("Access denied: Your IP is not allowed." + ip);
    }
});

// ---------------------------

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", $"My API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI as the root endpoint
});





app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

// Ensure Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


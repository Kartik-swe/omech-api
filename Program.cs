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

// Register the DatabaseHelper as a singleton
builder.Services.AddSingleton(new DatabaseHelper(connectionString));

// Register the DataService as a scoped service (if it needs to be scoped)
builder.Services.AddScoped<IDataService, DataService>();

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

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI as the root endpoint
});

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

// Ensure Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


//using omech.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Added New code
//// Configure CORS (Cross-Origin Resource Sharing)
//builder.Services.AddCors(options =>
//{
//    // Define a CORS policy
//    options.AddPolicy("AllowAllOrigins", builder =>
//        builder.AllowAnyOrigin()  // Allow requests from any origin
//               .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
//               .AllowAnyHeader());  // Allow any header
//});

//// Add services to the container.
//var configuration = builder.Configuration; var connectionString = configuration.GetConnectionString("db_dev_con");

//builder.Services.AddSingleton(new DatabaseHelper(connectionString)); builder.Services.AddControllers();

//// Register the DatabaseHelper as a singleton
//builder.Services.AddSingleton(new DatabaseHelper(connectionString));

//// Register the DataService as a scoped service (if it needs to be scoped)
//builder.Services.AddScoped<IDataService, DataService>();


////Added new code end
//var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("swagger/v1/swagger.json", "My API V1");
//    c.RoutePrefix = string.Empty; // Set Swagger UI as the root endpoint
//});

//app.UseCors("AllowAllOrigins");

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

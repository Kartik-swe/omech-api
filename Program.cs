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
var configuration = builder.Configuration; var connectionString = configuration.GetConnectionString("db_dev_con");

builder.Services.AddSingleton(new DatabaseHelper(connectionString)); builder.Services.AddControllers();

// Register the DatabaseHelper as a singleton
builder.Services.AddSingleton(new DatabaseHelper(connectionString));

// Register the DataService as a scoped service (if it needs to be scoped)
builder.Services.AddScoped<IDataService, DataService>();


//Added new code end
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

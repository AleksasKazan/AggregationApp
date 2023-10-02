using AggregationApp;
using Persistence;
using Domain;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new CompactJsonFormatter())
    .WriteTo.File(
        path: Path.Combine(AppContext.BaseDirectory, "logs/log-.txt"),
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information,
        outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
builder.Services.AddSingleton(loggerFactory);
builder.Services.AddLogging();

builder.Services.AddHostedService<Aggregation>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddDomain();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
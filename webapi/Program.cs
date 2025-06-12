using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger =  new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("Logs/Records.txt",rollingInterval:RollingInterval.Day).CreateLogger();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
////builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

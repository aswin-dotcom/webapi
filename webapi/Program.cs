using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using webapi;
using webapi.Data;
using webapi.Loging;
using webapi.Repository;
using webapi.Repository.IRepository;
//using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MapConfig));

// Add services to the container.

//Log.Logger =  new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("Logs/Records.txt",rollingInterval:RollingInterval.Day).CreateLogger();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();


builder.Services.AddSingleton<ILogging, Logging>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();
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

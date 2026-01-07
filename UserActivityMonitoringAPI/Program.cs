
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Quartz;
using Serilog;
using UserActivityMonitoringAPI.Data;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Jobs;
using UserActivityMonitoringAPI.Services;
using UserActivityMonitoringAPI.Services.Interfaces;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log",
        rollingInterval: RollingInterval.Day).CreateLogger();
    

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:7000");
// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(conf => conf.UseNpgsql(connection));
builder.Services.AddScoped<IUserActivityService, UserActivityService>();
builder.Services.AddScoped<IActivityTypeService, ActivityTypeService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

var jobKey = new JobKey("ActivityAnalysis" +
                        "Job");

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

   
    q.AddJob<ActivityAnalysisJob>(j => j.WithIdentity("ActivityAnalysisJob"));

    q.AddTrigger(t => t
        .WithIdentity("Every10SecondsTrigger")
        .ForJob("ActivityAnalysisJob")
        .WithCronSchedule("*/10 * * * * ?")
    );
});

builder.Services.AddQuartzHostedService(options =>
    options.WaitForJobsToComplete = true);

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseRouting();


app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.GetInfrastructure().GetService<IMigrator>()!.MigrateAsync();
app.Run();
using Microsoft.Extensions.Caching.Memory;
using Quartz;
using UserActivityMonitoringAPI.Data;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Jobs;

public class ActivityAnalysisJob:IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ActivityAnalysisJob> _logger;
    private readonly ICacheService _cache;

    public ActivityAnalysisJob(ApplicationDbContext dbContext, ILogger<ActivityAnalysisJob> logger, ICacheService cache)
    {
        _dbContext = dbContext;
        _logger = logger;
        _cache = cache;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting Job");
        
        //Primer: Ochistka starikh zapisey
        var oldDate = DateTime.UtcNow.AddDays(-30);
        var oldActivities = _dbContext.UserActivities.Where(a => a.CreatedOn < oldDate);
        _dbContext.UserActivities.RemoveRange(oldActivities);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Finished Job");
    }
}
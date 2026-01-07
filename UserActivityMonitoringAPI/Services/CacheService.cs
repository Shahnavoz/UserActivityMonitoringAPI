using Microsoft.Extensions.Caching.Memory;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Services;

public class CacheService(IMemoryCache cache):ICacheService
{
    private const string CacheKeyPrefix = "RecentActivities_";
    public List<UserActivity> GetRecentActivities(int userId)
    {
        cache.TryGetValue(CacheKeyPrefix + userId, out List<UserActivity> activities);
        return activities ?? new();
    }

    public void SetRecentActivities(int userId, List<UserActivity> activities)
    {
        var options=new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
        cache.Set(CacheKeyPrefix+userId, activities, options);
    }
}
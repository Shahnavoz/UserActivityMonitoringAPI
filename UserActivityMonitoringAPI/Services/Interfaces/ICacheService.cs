using UserActivityMonitoringAPI.Entities;

namespace UserActivityMonitoringAPI.Services.Interfaces;

public interface ICacheService
{
    List<UserActivity> GetRecentActivities(int userId);
    void SetRecentActivities(int userId,List<UserActivity> activities);
}
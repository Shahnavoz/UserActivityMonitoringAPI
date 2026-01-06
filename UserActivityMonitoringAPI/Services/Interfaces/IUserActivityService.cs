using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;

namespace UserActivityMonitoringAPI.Services.Interfaces;

public interface IUserActivityService
{
    Task<UserActivity> AddUserActivity(UserActivity userActivity);
    Task<List<UserActivity>> GetAllUserActivity();
    Task<Response<UserActivity>> GetUserActivity(long id);
    Task<Response<string>> UpdateUserActivity(UserActivity userActivity,long id);
    Task<Response<string>>  DeleteUserActivity(long id);
}
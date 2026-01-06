using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;

namespace UserActivityMonitoringAPI.Services.Interfaces;

public interface IActivityTypeService
{
    Task<Response<string>> AddActivityType(ActivityType activityType);
    Task<List<ActivityType>> GetAllActivityTypes();
    Task<Response<ActivityType>> GetActivityTypes(long id);
    Task<Response<string>> UpdateActivityType(ActivityType activityType,long id);
    Task<Response<string>> DeleteActivityType(long id);
    
    
    
}
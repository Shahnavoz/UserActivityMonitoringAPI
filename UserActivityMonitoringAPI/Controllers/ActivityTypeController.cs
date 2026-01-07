using Microsoft.AspNetCore.Mvc;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class ActivityTypeController(IActivityTypeService activityTypeService):ControllerBase
{
    [HttpGet("GetActivityTypes")]
    public async Task<List<ActivityType>> GetActivityTypes()
    {
        return await activityTypeService.GetAllActivityTypes();
    }

    [HttpGet("GetActivityType/{id}")]
    public async Task<Response<ActivityType>> GetActivityType(long id)
    {
        return await activityTypeService.GetActivityTypes(id);
    }

    [HttpPost("AddActivityType")]
    public async Task<Response<string>> AddActivityType(ActivityType activityType)
    {
        return await activityTypeService.AddActivityType(activityType);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateActivityType(ActivityType activityType,long id)
    {
        return await activityTypeService.UpdateActivityType(activityType, id);
    }

    [HttpDelete]
    public async Task<Response<string>> DeleteActivityType(long id)
    {
        return await activityTypeService.DeleteActivityType(id);
    }

}
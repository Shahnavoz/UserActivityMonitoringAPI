using Microsoft.AspNetCore.Mvc;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UserActivityController(IUserActivityService userActivityService):ControllerBase
{
    [HttpPost]
    public async Task<UserActivity> AddUserActivity(UserActivity userActivity)
    {
        return await userActivityService.AddUserActivity(userActivity);
    }

    [HttpGet("Get All UserActivities")]
    public async Task<List<UserActivity>> GetUserActivities()
    {
        return await userActivityService.GetAllUserActivity();
    }

    [HttpGet("{id}")]
    public async Task<Response<UserActivity>> GetUserActivity(long id)
    {
        return await userActivityService.GetUserActivity(id);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateUserActivity(UserActivity userActivity,long id)
    {
        return await userActivityService.UpdateUserActivity(userActivity, id);
    }

    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteUserActivity(long id)
    {
        return await userActivityService.DeleteUserActivity(id);
    }
    
    
    
}
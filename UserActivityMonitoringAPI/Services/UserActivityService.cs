using System.Net;
using Microsoft.EntityFrameworkCore;
using UserActivityMonitoringAPI.Data;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Services;

public class UserActivityService(ApplicationDbContext context,ILogger<UserActivityService> logger):IUserActivityService
{
    public async Task<UserActivity> AddUserActivity(UserActivity userActivity)
    {
        try
        {
            context.UserActivities.Add(userActivity);
            await context.SaveChangesAsync();
            logger.LogInformation("UserActivity added: {userActivityId}", userActivity.Id);
            return userActivity;
        }
        catch (Exception e)
        {
           logger.LogError(e, "Error adding UserActivity: {userActivityId}", userActivity.Id);
           return new UserActivity();
        }
    }

    public async Task<List<UserActivity>> GetAllUserActivity()
    {
        logger.LogInformation("Getting AllUserActivities");
        return await context.UserActivities.ToListAsync();
    }

    public async Task<Response<UserActivity>> GetUserActivity(long id)
    {
        try
        {
            context.UserActivities.Find(id);
            logger.LogInformation("Getting UserActivity: {userActivityId}", id);
            var uActivity = await context.UserActivities.FirstOrDefaultAsync();
            logger.LogInformation("UserActivity: {userActivityId}", id);

            return uActivity == null
                ? new Response<UserActivity>(HttpStatusCode.InternalServerError, "Some Error!", uActivity)
                : new Response<UserActivity>(HttpStatusCode.OK, "Found Succesfully!", uActivity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<UserActivity>(HttpStatusCode.InternalServerError, "Some Error!", null);
        }
    }

    public async Task<Response<string>> UpdateUserActivity(UserActivity userActivity, long id)
    {
        try
        {
            context.UserActivities.Find(id);
            logger.LogInformation("Updating UserActivity: {userActivityId}", id);
            var user=context.UserActivities.Update(userActivity);
            await context.SaveChangesAsync();
            logger.LogInformation("UserActivity updated: {userActivityId}", id);
            return new Response<string>(HttpStatusCode.OK, userActivity.Id.ToString());
            
             
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<string>(HttpStatusCode.InternalServerError, "Some Error!", null);
        }
    }

    public async Task<Response<string>> DeleteUserActivity(long id)
    {
        try
        {
          context.UserActivities.Find(id);
          logger.LogInformation("Deleting UserActivity: {userActivityId}", id);
          context.UserActivities.Remove(context.UserActivities.Find(id));
          await context.SaveChangesAsync();
          logger.LogInformation("UserActivity deleted: {userActivityId}", id);
          return new Response<string>(HttpStatusCode.OK, "Deleted Succesfully!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<string>(HttpStatusCode.InternalServerError, "Some Error!", null);
        }
    }
}
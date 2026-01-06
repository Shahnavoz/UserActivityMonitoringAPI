using System.Net;
using Microsoft.EntityFrameworkCore;
using UserActivityMonitoringAPI.Data;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Services;

public class ActivityTypeService(ApplicationDbContext  context,ILogger<ActivityTypeService>  logger):IActivityTypeService
{
    public async Task<Response<string>> AddActivityType(ActivityType activityType)
    {
        try
        {
          context.ActivityTypes.Add(activityType);
          await context.SaveChangesAsync();
          logger.LogInformation("ActivityType added :{activityTypeId}", activityType.Id);
          return activityType==null ? new Response<string>(HttpStatusCode.InternalServerError,"Some Error!")
              :  new Response<string>(HttpStatusCode.OK, activityType.Id.ToString());
          
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<string>(HttpStatusCode.InternalServerError,"Some Error!"+e.Message);
        }
    }

    public async Task<List<ActivityType>> GetAllActivityTypes()
    {
        try
        {
            var activities= context.ActivityTypes
                .ToListAsync();
            return await activities;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<ActivityType>();
        }
    }

    public async Task<Response<ActivityType>> GetActivityTypes(long id)
    {
        try
        {
            context.ActivityTypes.Find(id);
            logger.LogInformation("Getting ActivityTypes: {activityType}", id);
            var activityType = await context.ActivityTypes.FirstOrDefaultAsync();
            logger.LogInformation("ActivityType: {ActivityTypeId}", id);

            return activityType == null
                ? new Response<ActivityType>(HttpStatusCode.InternalServerError, "Some Error!", activityType)
                : new Response<ActivityType>(HttpStatusCode.OK, "Found Succesfully!", activityType);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<ActivityType>(HttpStatusCode.InternalServerError, "Some Error!", null);
        }
    }

    public async Task<Response<string>> UpdateActivityType(ActivityType activityType, long id)
    {
        try
        {
            context.ActivityTypes.Find(id);
            context.ActivityTypes.Update(activityType);
            await context.SaveChangesAsync();
            logger.LogInformation("ActivityType: {activityTypeId} updated", activityType.Id);
            return new Response<string>(HttpStatusCode.OK, activityType.Id.ToString());
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Response<string>(HttpStatusCode.InternalServerError, "Some Error!", null);
        }
    }

    public async Task<Response<string>> DeleteActivityType(long id)
    {
        var a=await context.ActivityTypes.Where(a => a.Id == id).ExecuteDeleteAsync();
        return a!=null ? new Response<string>(HttpStatusCode.OK, id.ToString()) 
            : new Response<string>(HttpStatusCode.InternalServerError,"Some Error!");
    }
}
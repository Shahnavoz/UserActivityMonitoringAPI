using System.Net;
using Microsoft.EntityFrameworkCore;
using UserActivityMonitoringAPI.Data;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Services;

public class ActivityTypeService(ApplicationDbContext _context,ILogger<IUserActivityService> _logger) : IActivityTypeService
{
    public async Task<Response<string>> AddActivityType(ActivityType activityType)
    {
        try
        {
            _context.ActivityTypes.Add(activityType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("ActivityType added with Id: {ActivityTypeId}", activityType.Id);

            return new Response<string>(HttpStatusCode.Created, activityType.Id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding ActivityType");
            return new Response<string>(HttpStatusCode.InternalServerError, "Failed to add ActivityType");
        }
    }

 
    public async Task<List<ActivityType>> GetAllActivityTypes()
    {
        try
        {
            return await _context.ActivityTypes.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all ActivityTypes");
            return new List<ActivityType>();  
        }
    }
   
    public async Task<Response<ActivityType>> GetActivityTypes(long id)
    {
        try
        {
            var activityType = await _context.ActivityTypes
                .FirstOrDefaultAsync(at => at.Id == id);

            if (activityType == null)
            {
                return new Response<ActivityType>(HttpStatusCode.NotFound, "ActivityType not found", null);
            }

            _logger.LogInformation("Retrieved ActivityType with Id: {ActivityTypeId}", id);
            return new Response<ActivityType>(HttpStatusCode.OK, "ActivityType found", activityType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ActivityType with Id: {Id}", id);
            return new Response<ActivityType>(HttpStatusCode.InternalServerError, "Error retrieving ActivityType", null);
        }
    }

    public async Task<Response<string>> UpdateActivityType( ActivityType activityType,long id)
    {
        if (id != activityType.Id)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Id mismatch");
        }

        try
        {
            var existing = await _context.ActivityTypes.FindAsync(id);
            if (existing == null)
            {
                return new Response<string>(HttpStatusCode.NotFound, "ActivityType not found");
            }

            existing.ActivityName = activityType.ActivityName;
            existing.Description = activityType.Description;

            _context.ActivityTypes.Update(existing);
            await _context.SaveChangesAsync();

            _logger.LogInformation("ActivityType updated with Id: {ActivityTypeId}", id);
            return new Response<string>(HttpStatusCode.OK, "ActivityType updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ActivityType with Id: {Id}", id);
            return new Response<string>(HttpStatusCode.InternalServerError, "Failed to update ActivityType");
        }
    }

    public async Task<Response<string>> DeleteActivityType(long id)
    {
        try
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);
            if (activityType == null)
            {
                return new Response<string>(HttpStatusCode.NotFound, "ActivityType not found");
            }

            _context.ActivityTypes.Remove(activityType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("ActivityType deleted with Id: {ActivityTypeId}", id);
            return new Response<string>(HttpStatusCode.OK, "ActivityType deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ActivityType with Id: {Id}", id);
            return new Response<string>(HttpStatusCode.InternalServerError, "Failed to delete ActivityType");
        }
    }
}
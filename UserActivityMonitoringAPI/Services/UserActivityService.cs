using System.Net;
using Microsoft.EntityFrameworkCore;
using UserActivityMonitoringAPI.Data;
using UserActivityMonitoringAPI.Entities;
using UserActivityMonitoringAPI.Responses;
using UserActivityMonitoringAPI.Services.Interfaces;

namespace UserActivityMonitoringAPI.Services;

public class UserActivityService(ApplicationDbContext _context,ILogger<IUserActivityService> _logger) : IUserActivityService
{

   
    public async Task<UserActivity> AddUserActivity(UserActivity userActivity)
    {
        try
        {
            _context.UserActivities.Add(userActivity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("UserActivity added with Id: {UserActivityId}, UserId: {UserId}, TypeId: {ActivityTypeId}",
                userActivity.Id, userActivity.Id, userActivity.ActivityTypeId);

            return userActivity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding UserActivity for UserId: {UserId}", userActivity.Id);
            throw;
        }
    }

   
    public async Task<List<UserActivity>> GetAllUserActivity()
    {
        _logger.LogInformation("Retrieving all UserActivities");
        return await _context.UserActivities
            .Include(ua => ua.ActivityType) 
            .OrderByDescending(ua => ua.CreatedOn)
            .ToListAsync();
    }

    // Получение по Id
    public async Task<Response<UserActivity>> GetUserActivity(long id)
    {
        try
        {
            var userActivity = await _context.UserActivities
                .Include(ua => ua.ActivityType)
                .FirstOrDefaultAsync(ua => ua.Id == id);

            if (userActivity == null)
            {
                _logger.LogWarning("UserActivity with Id {UserActivityId} not found", id);
                return new Response<UserActivity>(HttpStatusCode.NotFound, "UserActivity not found", null);
            }

            _logger.LogInformation("Found UserActivity with Id: {UserActivityId}", id);
            return new Response<UserActivity>(HttpStatusCode.OK, "UserActivity found", userActivity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting UserActivity with Id: {Id}", id);
            return new Response<UserActivity>(HttpStatusCode.InternalServerError, "Error retrieving UserActivity", null);
        }
    }

    public async Task<Response<string>> UpdateUserActivity(UserActivity userActivity,long id)
    {
        if (id != userActivity.Id)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Id in URL does not match Id in body");
        }

        try
        {
            var existing = await _context.UserActivities.FindAsync(id);
            if (existing == null)
            {
                return new Response<string>(HttpStatusCode.NotFound, "UserActivity not found");
            }

            // Обновляем только разрешённые поля (Timestamp обычно не меняем)]
            existing.Id = userActivity.Id;
            existing.ActivityTypeId = userActivity.ActivityTypeId;
            existing.ActivityType = userActivity.ActivityType;

            await _context.SaveChangesAsync();

            _logger.LogInformation("UserActivity updated with Id: {UserActivityId}", id);
            return new Response<string>(HttpStatusCode.OK, "UserActivity updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating UserActivity with Id: {Id}", id);
            return new Response<string>(HttpStatusCode.InternalServerError, "Failed to update UserActivity");
        }
    }

    public async Task<Response<string>> DeleteUserActivity(long id)
    {
        try
        {
            var userActivity = await _context.UserActivities.FindAsync(id);
            if (userActivity == null)
            {
                return new Response<string>(HttpStatusCode.NotFound, "UserActivity not found");
            }

            _context.UserActivities.Remove(userActivity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("UserActivity deleted with Id: {UserActivityId}", id);
            return new Response<string>(HttpStatusCode.OK, "UserActivity deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting UserActivity with Id: {Id}", id);
            return new Response<string>(HttpStatusCode.InternalServerError, "Failed to delete UserActivity");
        }
    }
}
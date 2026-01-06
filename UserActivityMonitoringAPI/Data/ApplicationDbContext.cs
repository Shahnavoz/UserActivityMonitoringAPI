

using Microsoft.EntityFrameworkCore;
using UserActivityMonitoringAPI.Entities;

namespace UserActivityMonitoringAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }
}
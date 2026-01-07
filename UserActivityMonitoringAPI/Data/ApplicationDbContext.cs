

using Microsoft.EntityFrameworkCore;
using UserActivityMonitoringAPI.Entities;

namespace UserActivityMonitoringAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserActivity>()
            .HasOne(ua => ua.ActivityType)
            .WithMany(at => at.UserActivities)
            .HasForeignKey(ua => ua.ActivityTypeId).OnDelete(DeleteBehavior.Cascade);
    }
}
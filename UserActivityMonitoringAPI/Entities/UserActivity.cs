namespace UserActivityMonitoringAPI.Entities;

public class UserActivity: BaseEntity
{
    public long ActivityTypeId { get; set; }
    public DateTime? CreatedOn { get; set; }
    
    public ActivityType? ActivityType { get; set; } = null!;
}
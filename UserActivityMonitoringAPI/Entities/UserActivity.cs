namespace UserActivityMonitoringAPI.Entities;

public class UserActivity: BaseEntity
{
    public ActivityType? ActivityType { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedOn { get; set; }
}
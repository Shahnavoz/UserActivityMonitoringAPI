using System.Diagnostics;

namespace UserActivityMonitoringAPI.Entities;

public class ActivityType:BaseEntity
{
    public string ActivityName { get; set; } = null!;
    public string Description { get; set; }=string.Empty;

    public List<UserActivity> UserActivities { get; set; } = new();



    //public ICollection<UserActivity> UserActivities { get; set; }=new List<UserActivity>();
}
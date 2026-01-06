using System.Diagnostics;

namespace UserActivityMonitoringAPI.Entities;

public class ActivityType:BaseEntity
{
    public string ActivityName { get; set; }
    public long UserActivityId { get; set; }
    public UserActivity UserActivity { get; set; }
    
    
    //public ICollection<UserActivity> UserActivities { get; set; }=new List<UserActivity>();
}
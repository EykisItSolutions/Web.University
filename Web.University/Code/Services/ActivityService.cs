using Web.University.Domain;

namespace Web.University;

#region Interface

public interface IActivityService
{
    Task SaveAsync(string activity, bool success = true);
}
#endregion

public class ActivityService(UniversityContext db, ICurrentUser currentUser,
    IHttpContextAccessor httpContextAccessor) : IActivityService
{
    #region Handlers

    public async Task SaveAsync(string activity, bool success = true)
    {
        var log = new ActivityLog()
        {
            UserId = currentUser.Id,
            Activity = activity,
            IpAddress = httpContextAccessor.IpAddress(),
            LogDate = DateTime.Now,
            Result = success ? "Success" : "Failed"
        };

        db.ActivityLogs.Add(log);

        await db.SaveChangesAsync();
    }

    #endregion
}
using Microsoft.EntityFrameworkCore;
using Web.University.Domain;

namespace Web.University;

#region Interface

public interface IViewedService
{
    Task SaveAsync(int whatId, string whatType, string whatName);
    int[] GetIds(string whatType);
}
#endregion

public class ViewedService(UniversityContext db, ICurrentUser currentUser) : IViewedService
{
    #region Handlers

    public async Task SaveAsync(int whatId, string whatType, string whatName)
    {
        // Logs a viewed record

        if (whatId != 0)
        {
            var viewed = await db.Vieweds.SingleOrDefaultAsync(v => v.WhatId == whatId && v.WhatType == whatType && v.UserId == currentUser.Id);
            if (viewed != null)
            {
                viewed.ViewDate = DateTime.Now;
                db.Vieweds.Update(viewed);
                await db.SaveChangesAsync();
            }
            else
            {
                viewed = new Viewed
                {
                    UserId = currentUser.Id ?? 0,
                    WhatId = whatId,
                    WhatType = whatType,
                    WhatName = whatName,
                    ViewDate = DateTime.Now
                };

                await db.Vieweds.AddAsync(viewed);
                await db.SaveChangesAsync();
            }
        }
    }

    public int[] GetIds(string whatType)
    {
        // Get most recently viewed items of a given type for currentuser

        var whatIds = db.Vieweds
            .FromSqlInterpolated($"SELECT WhatId FROM Viewed WHERE UserId = {currentUser.Id} AND WhatType = {whatType}")
            .Select(v => v.WhatId).ToArray();

        return whatIds;
    }

    #endregion
}
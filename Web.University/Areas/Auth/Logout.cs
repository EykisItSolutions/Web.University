using Microsoft.AspNetCore.Mvc;

namespace Web.University.Areas.Auth;

public class Logout : BaseModel
{
    #region Handlers

    public override async Task<IActionResult> GetAsync()
    {
        await _activityService.SaveAsync("Logout");

        return LocalRedirect("/");
    }

    #endregion
}


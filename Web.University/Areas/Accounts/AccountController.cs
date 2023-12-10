using Microsoft.AspNetCore.Mvc;

namespace Web.University.Areas.Accounts;

[Route("account")]
public class AccountController : Controller
{
    #region Pages

    [HttpGet]
    public async Task<IActionResult> Detail(Detail model) => await model.GetAsync();

    #endregion
}
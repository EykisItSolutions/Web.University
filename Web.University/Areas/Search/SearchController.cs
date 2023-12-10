using Microsoft.AspNetCore.Mvc;

namespace Web.University.Areas.Search;

[Menu("Search")]
[Route("search")]
public class SearchController : Controller
{
    #region Pages

    [HttpGet]
    public async Task<IActionResult> List(List model) => await model.GetAsync();

    #endregion
}
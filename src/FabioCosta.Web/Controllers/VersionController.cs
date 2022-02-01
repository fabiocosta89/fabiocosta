namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;

using Microsoft.AspNetCore.Mvc;

public class VersionController : Controller
{
    [Route("/version")]
    [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
    public IActionResult Index()
    {
        return View();
    }
}

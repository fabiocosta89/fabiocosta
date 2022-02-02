namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;

using Microsoft.AspNetCore.Mvc;

public class PrivacyController : Controller
{
    [Route("/privacy")]
    [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
    public IActionResult Index()
    {
        return View();
    }
}

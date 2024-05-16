namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;

using Microsoft.AspNetCore.Mvc;

[Route("/privacy")]
public class PrivacyController : Controller
{
    [Route("")]
    [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
    public IActionResult Index()
    {
        return View();
    }
}

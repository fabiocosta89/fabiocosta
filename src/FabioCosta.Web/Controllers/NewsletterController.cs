namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;

using Microsoft.AspNetCore.Mvc;

[Route("/newsletter")]
public class NewsletterController : Controller
{
    [Route("")]
    [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
    public IActionResult Index()
    {
        return View();
    }
}

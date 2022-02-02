namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;

using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    [ResponseCache(CacheProfileName = CacheConstants.Daily)]
    public IActionResult Index()
    {
        return View();
    }
}

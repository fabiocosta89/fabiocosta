namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Constants;

    using Microsoft.AspNetCore.Mvc;

    public class VersionController : Controller
    {
        [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
        public IActionResult Index()
        {
            return View();
        }
    }
}

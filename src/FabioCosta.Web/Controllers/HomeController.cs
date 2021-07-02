namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Constants;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = CacheConstants.Daily)]
        public IActionResult Index()
        {
            return View();
        }
    }
}

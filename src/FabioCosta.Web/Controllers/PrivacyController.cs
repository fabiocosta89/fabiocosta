namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class PrivacyController : Controller
{
    private readonly ILogger<PrivacyController> _logger;

    public PrivacyController(ILogger<PrivacyController> logger)
    {
        _logger = logger;
    }

    [Route("/privacy")]
    [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
    public IActionResult Index()
    {
        return View();
    }
}

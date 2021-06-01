namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Models;

    using Microsoft.AspNetCore.Mvc;

    using System.Diagnostics;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorController : Controller
    {
        [Route("/Error/{code:int}")]
        public IActionResult Index(int code = 0)
        {
            var model = new PageErrorModel
            {
                Code = code
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

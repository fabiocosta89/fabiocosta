namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using System;
    using System.Threading.Tasks;

    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogService _blogService;

        public BlogController(ILogger<BlogController> logger, IBlogService blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }

        /// <summary>
        /// Gets the blog archive with the given id.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _blogService.GetBlogPosts(HttpContext.User);

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}

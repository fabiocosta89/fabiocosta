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
        [Route("/blog")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _blogService.GetBlogPostsAsync(HttpContext.User);

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Gets the post with the given id.
        /// </summary>
        /// <param name="id">The unique post id</param>
        /// <param name="draft">If a draft is requested</param>
        [Route("/blog/{slug}")]
        public async Task<IActionResult> Post(string slug, bool draft = false)
        {
            try
            {
                var model = await _blogService.GetBlogPostBySlugAsync(slug, HttpContext.User, draft);

                return View(model);
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}

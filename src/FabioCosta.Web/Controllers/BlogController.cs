namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Piranha;
    using Piranha.AspNetCore.Services;
    using Piranha.Models;

    using System;
    using System.Threading.Tasks;

    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IApi _api;
        private readonly IModelLoader _loader;
        private readonly IApplicationService _webApp;

        public BlogController(ILogger<BlogController> logger, IApi api, IModelLoader loader,
            IApplicationService webApp)
        {
            _logger = logger;
            _api = api;
            _loader = loader;
            _webApp = webApp;
        }

        /// <summary>
        /// Gets the blog archive with the given id.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await GetBlogPosts();

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        #region Private Methods

        // Get list of blog posts
        private async Task<StandardArchive> GetBlogPosts()
        {
            var id = _webApp.CurrentPage.Id;
            var model = await _loader.GetPageAsync<StandardArchive>(id, HttpContext.User);
            model.Archive = await _api.Archives.GetByIdAsync<PostInfo>(id);

            return model;
        }

        #endregion
    }
}

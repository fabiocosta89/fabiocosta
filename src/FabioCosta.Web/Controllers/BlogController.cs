namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Constants;
    using FabioCosta.Web.Interfaces;
    using FabioCosta.Web.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Piranha.Models;

    using System;
    using System.ComponentModel.DataAnnotations;
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
        [ResponseCache(CacheProfileName = CacheConstants.Hourly)]
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
        [ResponseCache(CacheProfileName = CacheConstants.Hourly)]
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

        /// <summary>
        /// Saves the given comment and then redirects to the post.
        /// </summary>
        /// <param name="id">The unique post id</param>
        /// <param name="commentModel">The comment model</param>
        [HttpPost]
        [Route("/blog/{slug}/comment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePostComment(SaveCommentModel commentModel)
        {
            StandardPost model = null;
            TempData["Error"] = null;

            try
            {
                model = await _blogService.GetBlogPostByIdAsync(commentModel.Id, HttpContext.User);

                // validation of the url
                if (commentModel.CommentUrl != null)
                {
                    commentModel.CommentUrl = commentModel.CommentUrl.Trim();
                    if (!commentModel.CommentUrl.StartsWith("http://")
                        || commentModel.CommentUrl.StartsWith("https://"))
                    {
                        commentModel.CommentUrl = $"http://{commentModel.CommentUrl}";
                    }
                }

                // Create the comment
                var comment = new PostComment
                {
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "",
                    Author = commentModel.CommentAuthor,
                    Email = commentModel.CommentEmail,
                    Url = commentModel.CommentUrl,
                    Body = commentModel.CommentBody,
                    Created = DateTime.UtcNow
                };
                await _blogService.SaveCommentAsync(commentModel.Id, comment);

                return Redirect(model.Permalink + "#comments");
            }
            catch (ValidationException ve)
            {
                TempData["Error"] = ve.Message;
                return Redirect(model?.Permalink + "#leave-comment" ?? "/blog");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}

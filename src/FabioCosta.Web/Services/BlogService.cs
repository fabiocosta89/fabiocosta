namespace FabioCosta.Web.Services
{
    using FabioCosta.Web.Interfaces;
    using FabioCosta.Web.Models;

    using Piranha;
    using Piranha.AspNetCore.Services;
    using Piranha.Models;

    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class BlogService : IBlogService
    {
        private readonly IApi _api;
        private readonly IModelLoader _loader;
        private readonly IApplicationService _webApp;

        public BlogService(IApi api, IModelLoader loader, IApplicationService webApp)
        {
            _api = api;
            _loader = loader;
            _webApp = webApp;
        }

        /// <summary>
        /// Get list of blog posts
        /// </summary>
        /// <param name="user">User Context</param>
        /// <returns>List of blog posts</returns>
        public async Task<StandardArchive> GetBlogPostsAsync(ClaimsPrincipal user)
        {
            var id = _webApp.CurrentPage.Id;
            var model = await _loader.GetPageAsync<StandardArchive>(id, user);
            model.Archive = await _api.Archives.GetByIdAsync<PostInfo>(id);

            return model;
        }

        /// <summary>
        /// Get a list of the blog posts, by the page slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public async Task<StandardArchive> GetBlogPostsByPageSlugAsync(string slug)
        {
            var model = await _api.Pages.GetBySlugAsync<StandardArchive>(slug);
            model.Archive = await _api.Archives.GetByIdAsync<PostInfo>(model.Id);

            return model;
        }

        /// <summary>
        /// Get a post by it's slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="user"></param>
        /// <param name="draft"></param>
        /// <returns></returns>
        public async Task<StandardPost> GetBlogPostBySlugAsync(string slug, ClaimsPrincipal user, bool draft)
        {
            var id = _webApp.CurrentPage.Id;
            var post = await _api.Posts.GetBySlugAsync<StandardPost>(id, slug);

            if (post.IsCommentsOpen)
            {
                post.Comments = await _api.Posts.GetAllCommentsAsync(post.Id, true);

                post.Comments = post.Comments.OrderBy(c => c.ContentId);
            }

            return post;
        }

        /// <summary>
        /// Get a post by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<StandardPost> GetBlogPostByIdAsync(Guid id, ClaimsPrincipal user)
        {
            return await _loader.GetPostAsync<StandardPost>(id, user);
        }

        /// <summary>
        /// Save a new comment and verify if it should be approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task SaveCommentAsync(Guid id, PostComment comment)
        {
            await _api.Posts.SaveCommentAndVerifyAsync(id, comment);
        }
    }
}

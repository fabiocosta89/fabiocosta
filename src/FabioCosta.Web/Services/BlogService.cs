namespace FabioCosta.Web.Services
{
    using FabioCosta.Web.Interfaces;
    using FabioCosta.Web.Models;

    using Piranha;
    using Piranha.AspNetCore.Services;
    using Piranha.Models;

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

        public async Task<StandardPost> GetBlogPostBySlugAsync(string slug, ClaimsPrincipal user, bool draft)
        {
            var id = _webApp.CurrentPage.Id;
            var post = await _api.Posts.GetBySlugAsync<StandardPost>(id, slug);

            if (post.IsCommentsOpen)
            {
                post.Comments = await _api.Posts.GetAllCommentsAsync(post.Id, true);
            }

            return post;
        }
    }
}

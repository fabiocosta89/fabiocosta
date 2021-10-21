namespace FabioCosta.Web.Interfaces
{
    using FabioCosta.Web.Models;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IBlogService
    {
        /// <summary>
        /// Get list of blog posts
        /// </summary>
        /// <param name="user">User Context</param>
        /// <returns>List of blog posts</returns>
        Task<StandardArchive> GetBlogPostsAsync(ClaimsPrincipal user);

        Task<StandardPost> GetBlogPostBySlugAsync(string slug, ClaimsPrincipal user, bool draft);
    }
}

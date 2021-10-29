namespace FabioCosta.Web.Interfaces
{
    using FabioCosta.Web.Models;

    using Piranha.Models;

    using System;
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

        /// <summary>
        /// Get a list of the blog posts, by the page slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        Task<StandardArchive> GetBlogPostsByPageSlugAsync(string slug);

        /// <summary>
        /// Get a post by it's slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="user"></param>
        /// <param name="draft"></param>
        /// <returns></returns>
        Task<StandardPost> GetBlogPostBySlugAsync(string slug, ClaimsPrincipal user, bool draft);

        /// <summary>
        /// Get a post by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<StandardPost> GetBlogPostByIdAsync(Guid id, ClaimsPrincipal user);

        /// <summary>
        /// Save a new comment and verify if it should be approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task SaveCommentAsync(Guid id, PostComment comment);
    }
}

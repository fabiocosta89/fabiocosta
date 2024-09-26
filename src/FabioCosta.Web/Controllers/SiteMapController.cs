namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Interfaces;

using Microsoft.AspNetCore.Mvc;

using SimpleMvcSitemap;

using System.Collections.Generic;
using System.Threading.Tasks;

[Route("/")]
public class SiteMapController : Controller
{
    private readonly ISitemapProvider _sitemapProvider;
    private readonly IBlogService _blogService;

    public SiteMapController(ISitemapProvider sitemapProvider, IBlogService blogService)
    {
        _sitemapProvider = sitemapProvider;
        _blogService = blogService;
    }

    [Route("SiteMap.xml")]
    [ResponseCache(CacheProfileName = CacheConstants.Daily)]
    public async Task<IActionResult> Index()
    {
        const string indexString = "Index";
        string scheme = HttpContext.Request.Scheme == "http" ? $"{HttpContext.Request.Scheme}s" : HttpContext.Request.Scheme;

        var nodes = new List<SitemapNode>
            {
                new (Url.Action(indexString,"Home", null, scheme))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 1.0M
                },
                new (Url.Action(indexString,"Privacy", null, scheme))
                {
                    ChangeFrequency = ChangeFrequency.Yearly,
                    Priority = 0.3M
                },
                new (Url.Action(indexString, "Blog", null, scheme))
                {
                    ChangeFrequency = ChangeFrequency.Daily,
                    Priority = 0.8M
                }
            };

        // Blog posts
        var posts = await _blogService.GetBlogPostsByPageSlugAsync("blog");
        foreach (var post in posts.Archive.Posts)
        {
            var node = new SitemapNode(Url.Action("Post", "Blog", new { slug = post.Slug }, scheme))
            {
                ChangeFrequency = ChangeFrequency.Weekly,
                Priority = 1.0M
            };

            nodes.Add(node);
        }

        return _sitemapProvider.CreateSitemap(new SitemapModel(nodes));
    }
}

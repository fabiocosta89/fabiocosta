namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Interfaces;

using Microsoft.AspNetCore.Mvc;

using SimpleMvcSitemap;

using System.Collections.Generic;
using System.Threading.Tasks;

public class SiteMapController : Controller
{
    private readonly ISitemapProvider _sitemapProvider;
    private readonly IBlogService _blogService;

    public SiteMapController(ISitemapProvider sitemapProvider, IBlogService blogService)
    {
        _sitemapProvider = sitemapProvider;
        _blogService = blogService;
    }

    [Route("/SiteMap.xml")]
    [ResponseCache(CacheProfileName = CacheConstants.Daily)]
    public async Task<IActionResult> Index()
    {
        var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index","Home"))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 1.0M
                },
                new SitemapNode(Url.Action("Index","Privacy"))
                {
                    ChangeFrequency = ChangeFrequency.Yearly,
                    Priority = 0.3M
                },
                new SitemapNode(Url.Action("Index","Version"))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    Priority = 0.5M
                },
                new SitemapNode(Url.Action("Index","Blog"))
                {
                    ChangeFrequency = ChangeFrequency.Daily,
                    Priority = 0.8M
                }
            };

        // Blog posts
        var posts = await _blogService.GetBlogPostsByPageSlugAsync("blog");
        foreach (var post in posts.Archive.Posts)
        {
            var node = new SitemapNode(Url.Action("Post", "Blog", new { slug = post.Slug }))
            {
                ChangeFrequency = ChangeFrequency.Weekly,
                Priority = 1.0M
            };

            nodes.Add(node);
        }

        return _sitemapProvider.CreateSitemap(new SitemapModel(nodes));
    }
}

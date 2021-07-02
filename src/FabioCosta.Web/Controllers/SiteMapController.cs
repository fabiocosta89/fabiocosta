namespace FabioCosta.Web.Controllers
{
    using FabioCosta.Web.Constants;

    using Microsoft.AspNetCore.Mvc;

    using SimpleMvcSitemap;

    using System;
    using System.Collections.Generic;

    public class SiteMapController : Controller
    {
        private readonly ISitemapProvider _sitemapProvider;

        public SiteMapController(ISitemapProvider sitemapProvider)
        {
            _sitemapProvider = sitemapProvider;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = CacheConstants.Weekly)]
        public IActionResult Index()
        {
            DateTime now = DateTime.UtcNow.ToLocalTime();

            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index","Home"))
                { 
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = now,
                    Priority = 1.0M
                },
                new SitemapNode(Url.Action("Index","Privacy"))
                {
                    ChangeFrequency = ChangeFrequency.Yearly,
                    LastModificationDate = now,
                    Priority = 0.3M
                },
                new SitemapNode(Url.Action("Index","Version"))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = now,
                    Priority = 0.5M
                }
            };

            //return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
            return _sitemapProvider.CreateSitemap(new SitemapModel(nodes));
        }
    }
}

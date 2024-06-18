namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Interfaces;
using FabioCosta.Web.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

[Route("/")]
public class FeedController : Controller
{
    private readonly IBlogService _blogService;

    public FeedController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [Route("feed")]
    [ResponseCache(CacheProfileName = CacheConstants.Daily)]
    public async Task<IActionResult> Index()
    {
        string scheme = HttpContext.Request.Scheme == "http" ? $"{HttpContext.Request.Scheme}s" : HttpContext.Request.Scheme;

        var urlBase = $"{scheme}://{HttpContext.Request.Host.Value}";
        // feed object creation
        var feed = new SyndicationFeed(
            "FabioCosta",
            "Hello, my name is Fabio and I am a Fullstack Developer.This is my Portfolio Website.",
            new Uri(urlBase),
            "RSSUrl",
            DateTime.Now)
        {
            Language = "en-us",
            Copyright = new TextSyndicationContent($"{DateTime.Now.Year} Fábio Costa")
        };
        XNamespace atom = "http://www.w3.org/2005/Atom";
        var element = new XElement(atom + "link",
            new XAttribute(XNamespace.Xmlns + "atom", atom.ToString()),
            new XAttribute("href", $"{urlBase}/feed"),
            new XAttribute("rel", "self"),
            new XAttribute("type", "application/rss+xml"));
        feed.ElementExtensions.Add(element);

        // get all blog posts
        var items = new List<SyndicationItem>();
        StandardArchive posts = await _blogService.GetBlogPostsByPageSlugAsync("blog");
        foreach (var post in posts.Archive.Posts)
        {
            var postUrl = Url.Action("Post", "Blog", new { slug = post.Slug }, scheme);
            var title = post.Title;
            var description = post.Excerpt;
            items.Add(new SyndicationItem(title, description, new Uri(postUrl), post.Slug, post.Created));
        }
        feed.Items = items;

        // generate response
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };
        using var stream = new MemoryStream();
        using (var xmlWriter = XmlWriter.Create(stream, settings))
        {
            var rssFormatter = new Rss20FeedFormatter(feed, false);
            rssFormatter.WriteTo(xmlWriter);
            await xmlWriter.FlushAsync();
        }
        return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
    }

    [Route("rss")]
    public RedirectToActionResult RedirectToFeed()
    {
        return RedirectToActionPermanent("Index");
    }
}

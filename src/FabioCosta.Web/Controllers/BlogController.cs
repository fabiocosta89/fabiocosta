﻿namespace FabioCosta.Web.Controllers;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Interfaces;
using FabioCosta.Web.Models;

using Microsoft.AspNetCore.Mvc;

using Piranha.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

[Route("/blog")]
public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly IExternalService _externalService;

    public BlogController(IBlogService blogService, IExternalService externalService)
    {
        _blogService = blogService;
        _externalService = externalService;
    }

    /// <summary>
    /// Gets the blog archive with the given id.
    /// </summary>
    [Route("page/{number}")]
    [Route("")]
    [ResponseCache(CacheProfileName = CacheConstants.Hourly)]
    public async Task<IActionResult> Index(int? number = null)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            if (!number.HasValue) number = 1;

            StandardArchive posts = await _blogService.GetBlogPostsAsync(HttpContext.User, number.Value);
            var postsModel = new PostList
            {
                Posts = posts
            };

            return View(postsModel);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [Route("category/{category}")]
    [Route("tag/{tag}")]
    public async Task<IActionResult> FilteredPosts(string category = null, string tag = null)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            var archive = await _blogService.GetBlogPostsFilteredAsync(HttpContext.User, category, tag);

            var model = new FilteredPosts
            {
                StandardArchive = archive,
                Category = category,
                Tag = tag
            };

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
    [Route("{slug}")]
    [ResponseCache(CacheProfileName = CacheConstants.Hourly)]
    public async Task<IActionResult> Post(string slug, bool draft = false)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            var model = await _blogService.GetBlogPostBySlugAsync(slug, HttpContext.User, draft);
            if (model == null) return NotFound();

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
    [Route("{slug}/comment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePostComment(SaveCommentModel commentModel, string slug)
    {
        const string error = "Error";
        TempData[error] = null;
        string redirectUrl = $"/blog/{slug}#leave-comment";
        string captchaError = "The captcha is mandatory!";

        if (!ModelState.IsValid)
        {
            return Redirect(redirectUrl);
        }

        try
        {
            if (!Request.Form.ContainsKey("g-recaptcha-response"))
            {
                TempData[error] = captchaError;
                return Redirect(redirectUrl);
            }

            var captcha = Request.Form["g-recaptcha-response"].ToString();
            if (string.IsNullOrEmpty(captcha))
            {
                TempData[error] = captchaError;
                return Redirect(redirectUrl);
            }

            if (!await _externalService.IsCaptchaValid(captcha))
            {
                TempData[error] = captchaError;
                return Redirect(redirectUrl);
            }

            StandardPost model = await _blogService.GetBlogPostByIdAsync(commentModel.Id, HttpContext.User);

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
                UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers.UserAgent.ToString() : "",
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
            TempData[error] = ve.Message;
            return Redirect(redirectUrl);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}

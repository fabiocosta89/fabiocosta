namespace FabioCosta.Web.ViewComponents;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

public class CookieConsentViewComponent : ViewComponent
{
    [ResponseCache(CacheProfileName = CacheConstants.NoCache)]
    public IViewComponentResult Invoke()
    {
        var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
        CookieConsentModel cookieConsentModel = new()
        {
            ShowBanner = !consentFeature?.CanTrack ?? false,
            CookieString = consentFeature?.CreateConsentCookie()
        };

        return View(cookieConsentModel);
    }
}

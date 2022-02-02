namespace FabioCosta.Web.Security.Head.Csp;

using Microsoft.AspNetCore.Builder;

using System;

public static class CspMiddlewareExtensions
{
    public static IApplicationBuilder UseCsp(
        this IApplicationBuilder app, Action<CspOptionsBuilder> builder)
    {
        var newBuilder = new CspOptionsBuilder();
        builder(newBuilder);

        var options = newBuilder.Build();
        return app.UseMiddleware<CspMiddleware>(options);
    }
}

namespace FabioCosta.Web;

using FabioCosta.Web.Constants;
using FabioCosta.Web.Interfaces;
using FabioCosta.Web.Security.Head.Csp;
using FabioCosta.Web.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Piranha;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.SQLServer;
using Piranha.Manager.Editor;

using SimpleMvcSitemap;

using System;
using System.IO.Compression;
using System.Linq;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Application Insights
        services.AddApplicationInsightsTelemetry();

        // Services
        services.AddSingleton<ISitemapProvider, SitemapProvider>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddHttpClient<IExternalService, ExternalService>(c =>
        {
            c.BaseAddress = new Uri($"{Configuration.GetValue(typeof(string), "Captcha:ValidationUrl")}");
        });

        // Service setup
        services.AddPiranha(options =>
        {
            options.UseCms(opt =>
            {
                opt.UseSiteRouting = false;
                opt.UseAliasRouting = false;
                opt.UseStartpageRouting = false;
                opt.UseArchiveRouting = false;
                opt.UsePostRouting = false;
                opt.UsePageRouting = false;
                opt.UseSitemapRouting = false;
            });
            options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
            options.UseImageSharp();
            options.UseManager();
            options.UseTinyMCE();
            options.UseMemoryCache();
            options.UseEF<SQLServerDb>(db =>
                db.UseSqlServer(Configuration.GetConnectionString("Database")));
            options.UseIdentityWithSeed<IdentitySQLServerDb>(db =>
                db.UseSqlServer(Configuration.GetConnectionString("Database")));
        });

        services.AddCors();

        services.AddControllersWithViews();

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes =
                ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "image/svg+xml" });
        });

        services.AddWebOptimizer(pipeline =>
        {
            pipeline.AddScssBundle("/css/bundle.css",
                "/css/template.css",
                "/sass/site.scss");

            pipeline.AddJavaScriptBundle("/js/bundleSite.js",
                "/js/site.js");

            pipeline.AddJavaScriptBundle("/js/bundleHome.js",
                "/js/site.js",
                "/js/home.js");
        });

        services.AddMvc(options =>
        {
            options.CacheProfiles.Add(CacheConstants.NoCache, new CacheProfile()
            {
                Duration = DurationConstants.DurationInSeconds.None,
                Location = ResponseCacheLocation.None,
                NoStore = true
            });
            options.CacheProfiles.Add(CacheConstants.Hourly, new CacheProfile()
            {
                Duration = DurationConstants.DurationInSeconds.Hour // 1 hour
                });
            options.CacheProfiles.Add(CacheConstants.Daily, new CacheProfile()
            {
                Duration = DurationConstants.DurationInSeconds.Day // 1 day
                });
            options.CacheProfiles.Add(CacheConstants.Weekly, new CacheProfile()
            {
                Duration = DurationConstants.DurationInSeconds.Week // 7 days
                });
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddResponseCaching();
        services.AddMemoryCache();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
    {
        app.UseResponseCompression();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStatusCodePagesWithReExecute("/Error/{0}");

        // Initialize Piranha
        App.Init(api);

        // Build content types
        new ContentTypeBuilder(api)
            .AddAssembly(typeof(Startup).Assembly)
            .Build()
            .DeleteOrphans();

        // Configure Tiny MCE
        EditorConfig.FromFile("editorconfig.json");

        // Middleware setup
        app.UsePiranha(options =>
        {
            options.UseManager();
            options.UseTinyMCE();
            options.UseIdentity();
        });

        // Content-Security-Policy
        app.UseCsp(builder =>
        {
            builder.Defaults
                   .AllowAny();

            builder.Scripts
                   .AllowSelf()
                   .Allow("https://plausible.fabiocosta.eu")
                   .Allow("https://code.jquery.com")
                   .Allow("https://cdn.jsdelivr.net")
                   .Allow("https://cdnjs.cloudflare.com")
                   .Allow("https://www.googletagmanager.com")
                   .Allow("https://unpkg.com");

            builder.Styles
                   .AllowSelf()
                   .Allow("https://cdn.jsdelivr.net")
                   .Allow("https://cdnjs.cloudflare.com")
                   .Allow("https://unpkg.com");

            builder.Fonts
                   .AllowSelf()
                   .Allow("https://cdnjs.cloudflare.com");

            builder.Images
                   .AllowAny();
        });

        app.UseHttpsRedirection();
        app.UseWebOptimizer();
        app.UseStaticFiles();

        app.UseCookiePolicy();

        app.UseRouting();

        app.UseCors();

        app.UseAuthorization();

        app.UseResponseCompression();
        app.UseResponseCaching();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}

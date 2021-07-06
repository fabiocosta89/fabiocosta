namespace FabioCosta.Web
{
    using FabioCosta.Web.Constants;
    using FabioCosta.Web.Security.Head.Csp;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using SimpleMvcSitemap;

    using System.IO.Compression;
    using System.Linq;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Application Insights
            services.AddApplicationInsightsTelemetry();

            // Services
            services.AddSingleton<ISitemapProvider, SitemapProvider>();

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
                    "/css/templatemo-digital-trend.css",
                    "/css/site.scss");

                pipeline.AddJavaScriptBundle("/js/bundleSite.js",
                    "/js/site.js");

                pipeline.AddJavaScriptBundle("/js/bundleHome.js",
                    "/js/site.js",
                    "/js/home.js");
            });

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add(CacheConstants.Hourly, new CacheProfile()
                {
                    Duration = DurationConstants.DurationInSeconds.Hour // 1 hour
                });;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            // Content-Security-Policy
            app.UseCsp(builder =>
            {
                builder.Defaults
                       .AllowSelf()
                       .Allow("https://analytics.google.com/");

                builder.Scripts
                       .AllowSelf()
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
}

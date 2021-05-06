namespace FabioCosta.Web
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.IO.Compression;

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
            services.AddControllersWithViews();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml", "text / json", "application/json", "text/css", "text/html" });
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddScssBundle("/css/bundle.css",
                    "/css/templatemo-digital-trend.css",
                    "/css/site.scss");
                pipeline.AddCssBundle("/css/bundleLibs.css",
                    "/lib/twitter-bootstrap/css/bootstrap.css",
                    "/lib/aos/aos.css",
                    "/lib/OwlCarousel2/assets/owl.carousel.css",
                    "/lib/OwlCarousel2/assets/owl.theme.default.css");
                pipeline.AddCssBundle("/css/bundleFonts.css",
                    "/lib/font-awesome/css/fontawesome.css",
                    "/lib/font-awesome/css/solid.css",
                    "/lib/font-awesome/css/brands.css");

                pipeline.AddJavaScriptBundle("/js/bundle.js",
                    "/js/site.js");
                pipeline.AddJavaScriptBundle("/js/bundleLibs.js",
                    "/lib/jquery/jquery.js",
                    "/lib/twitter-bootstrap/js/bootstrap.js",
                    "/lib/aos/aos.js",
                    "/lib/OwlCarousel2/owl.carousel.js",
                    "/lib/smoothscroll/SmoothScroll.js",
                    "/lib/rellax/rellax.js");
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseWebOptimizer();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

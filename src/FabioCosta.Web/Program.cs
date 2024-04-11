namespace FabioCosta.Web;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json", true)
                    .Build();

                webBuilder.UseStartup<Startup>();
                webBuilder.UseSentry(sentry =>
                {                   
                    sentry.Dsn = configuration.GetValue(typeof(string), "Sentry:Dsn")?.ToString();
                    sentry.Debug = true;
                    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                    sentry.TracesSampleRate = 0.2;
                });
            });
}

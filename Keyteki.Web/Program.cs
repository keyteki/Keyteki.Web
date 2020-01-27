namespace Keyteki.Web
{
    using System;
    using Keyteki.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NLog.Web;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<KeytekiDbContext>();
                        context.Database.Migrate();
/*
                        await SeedData.Initialize(scope, context);
                        await ThronetekiSeedData.Initialize(services);
*/
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occurred seeding the database.");

                        throw;
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred starting the website");

                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                            logging.SetMinimumLevel(LogLevel.Trace);
                        })
                        .UseNLog();
                });
    }
}

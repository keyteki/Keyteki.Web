namespace Keyteki.Web
{
    using System.Diagnostics.CodeAnalysis;
    using CrimsonDev.Gameteki.Api.Helpers;
    using CrimsonDev.Gameteki.Api.Services;
    using CrimsonDev.Gameteki.Data.Models.Config;
    using Keyteki.Data;
    using Keyteki.Web.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var general = Configuration.GetSection("General").Get<GametekiApiOptions>();

            if (general.DatabaseProvider == "mssql")
            {
                services.AddDbContext<KeytekiDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                services.AddDbContext<KeytekiDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddGameteki(Configuration);

            services.AddTransient<IKeytekiDbContext, KeytekiDbContext>();
            services.AddTransient<IUserService, KeytekiUserService>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGameteki();

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}

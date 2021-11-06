using System.Linq;
using System.Net.Mime;
using DeadLetterRedemption.Server.Hub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeadLetterRedemption.Server
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(
                options => options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { MediaTypeNames.Application.Octet }));

            services.AddControllersWithViews();
            // services.AddRazorPages();

            services.AddSignalR(options => options.EnableDetailedErrors = true).AddMessagePackProtocol();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            app.UseRouting();
            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyHeader();
                c.AllowAnyMethod();
            });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                //endpoints.MapControllers();
                endpoints.MapHub<AppHub>("/app");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}

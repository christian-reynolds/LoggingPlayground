using Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
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
            services.AddRazorPages();

            // https://github.com/mo-esmp/serilog-enrichers-clientinfo
            // You need to register the IHttpContextAccessor singleton
            // so the enrichers have access to the requests HttpContext to extract client IP and client agent.
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            // https://github.com/serilog/serilog-aspnetcore#request-logging
            // Middleware for smarter HTTP request logging
            app.UseSerilogRequestLogging(opts =>
                opts.EnrichDiagnosticContext = LogEnricher.EnrichFromRequest);

            //app.UseSerilogRequestLogging(options =>
            //{
            //    // Attach additional properties to the request completion event
            //    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            //    {
            //        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            //        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            //    };
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

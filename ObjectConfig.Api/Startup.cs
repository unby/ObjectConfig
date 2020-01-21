using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ObjectConfig.Exceptions;
using ObjectConfig.Features;
using System;
using System.Net.Http;

namespace ObjectConfig
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
            services.AddControllersWithViews().ControllersRegister();

            services.FeaturesRegister(a => a.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddProblemDetails(ConfigureProblemDetails);
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            // This is the default behavior; only include exception details in a development environment.
            // todo override initial
            options.IncludeExceptionDetails = ctx => true;

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));

            options.Map<ObjectConfigBaseException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            options.Map<NotFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status404NotFound));
            options.Map<ForbidenException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status403Forbidden));
            options.Map<RequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
            options.Map<OperationException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            app.UseProblemDetails();
        }
    }
}

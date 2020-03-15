using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ObjectConfig.Data;
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
            services.AddProblemDetails(ConfigureProblemDetails).AddMvcCore(r => r.EnableEndpointRouting = false).AddJsonOptions(json => { json.JsonSerializerOptions.IgnoreNullValues = true; });
            services.AddControllersWithViews().ControllersRegister();

            services.FeaturesRegister();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ObjectConfig API", Version = "1" });
            });

            services.AddHealthChecks();
            if (!Configuration.GetValue<bool>("IsUnitTest"))
            {
                services.AddHealthChecksUI();

                services.AddDbContext<ObjectConfigContext>(a => a.UseSqlServer(@"Data Source=localhost;Initial Catalog=ObjectConfig;Integrated Security=True;", opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
            }


        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = ctx => true;
            // logging all bad response
            options.ShouldLogUnhandledException = (x, y, z) => true;

            // domain exceptions
            options.Map<RequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest));
            options.Map<NotFoundException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status404NotFound));
            options.Map<ForbidenException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status403Forbidden));
            options.Map<OperationException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));

            options.Map<DbUpdateException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status409Conflict));
            options.Map<EntityException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status409Conflict));

            // infrasracture exceptions
            options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));
            options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));

            options.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            if (!Configuration.GetValue<bool>("IsUnitTest"))
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "ObjectConfig(V1)");
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // https://stackoverflow.com/questions/50633863/how-to-set-a-default-port-for-asp-net-core-angular-app/50980371#50980371
                    spa.UseReactDevelopmentServer(npmScript: "start");

                    // https://medium.com/@faisal_/live-reloading-angular-application-with-asp-net-core-in-visual-studio-2017-957619f31008
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}

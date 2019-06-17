using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using nCubed.MVCCore.Services.TypeHelperService;
using nCubed.MVCCore.Sample.Services;
using Microsoft.AspNetCore.Http;

namespace nCubed.MVCCore.Sample
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
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonFormatters(options =>
                {
                    options.NullValueHandling = NullValueHandling.Ignore;
                });

            ServicesInjection(services);
        }

        public static void ServicesInjection(IServiceCollection services)
        {
            // Paging dependencies
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<TypeHelperService, TypeHelperService>();
            services.AddScoped<IUrlHelper, UrlHelper>(factory =>
            {
                var context = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(context);
            });
            //services.AddTransient<IPropertyMappingService, IPropertyMappingService>();
            // Add application required dependencies
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

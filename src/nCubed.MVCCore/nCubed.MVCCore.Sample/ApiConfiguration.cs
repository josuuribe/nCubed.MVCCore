using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using nCubed.MVCCore.Services.TypeHelperService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nCubed.MVCCore.Sample
{
    public class ApiConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //.AddAuthorization(Policies.Configure)
                .AddJsonFormatters(options =>
                {
                    options.NullValueHandling = NullValueHandling.Ignore;
                });
            ServicesInjection(services);
            // Repositories
            //services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();

            // Services
            //services.AddScoped<IScopedService, ScopedService>();

            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            //services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();

            // Authorization handlers
            //services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
            //services.AddSingleton<IAuthorizationHandler, HasBadgeHandler>();
            //services.AddSingleton<IAuthorizationHandler, HasTemporaryPassHandler>();
            //services.AddSingleton<IAuthorizationHandler, OwnedProductHandler>();
        }

        public static void ServicesInjection(IServiceCollection services)
        {
            // Paging dependencies
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<TypeHelperService, TypeHelperService>();
            services.AddScoped<IUrlHelper, UrlHelper>(factory =>
            {
                var context = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(context);
            });
            //services.AddTransient<IPropertyMappingService, IPropertyMappingService>();
            // Add application required dependencies
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}

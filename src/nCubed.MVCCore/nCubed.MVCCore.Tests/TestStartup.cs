using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using nCubed.MVCCore.Sample;
using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.Tests
{
    public class TestStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ApiConfiguration.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            ApiConfiguration.Configure(app);
        }
    }
}

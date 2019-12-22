using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;
using FundamentalsOfDocker.ch06.Library;
using Jaeger;

namespace FundamentalsOfDocker.ch06.api
{
    public class Startup
    {
        private static readonly Tracer Tracer = Tracing.Init("Webservice");
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            GlobalTracer.Register(Tracer);
            services.AddOpenTracing();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        // private static  Tracer GetTracer(string serviceName)
        // {
        //     var serviceCollection = new ServiceCollection();
        //     serviceCollection.AddLogging(builder => builder.AddConsole());
        //     using (var serviceProvider = serviceCollection.BuildServiceProvider())
        //     {
        //         var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        //         var tracer = Tracing.Init(serviceName, loggerFactory);
        //         return tracer;
        //     }
        // }
    }
}

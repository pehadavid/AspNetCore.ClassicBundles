using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.ClassicBundles.Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseRouting();
            app.UseBundles(env,  bundles =>
            {
               

                //you can enable/disable min mode here :
                bundles.IsMinMode = true;
                bundles.DontThrowOnMissingFile = true; //dont throw when a file in missing in bundles
                var b1 = bundles.AddAsync(new ScriptBundle("~/js/my-little-bundle.js")
                    .Include("~/lib/jquery/dist/jquery.js")
                    .Include("~/lib/bootstrap/dist/js/bootstrap.js")
                    .Include("~/js/site.js"));

                var b2 = bundles.AddAsync(new StyleBundle("~/css/my-css-bundle.css")
                    .Include("~/lib/bootstrap/dist/css/bootstrap.css"
                        , "~/css/site.css"));

                Task.WaitAll(b1, b2);
                app.UseStaticFiles();

            });
   
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            
            app.UseEndpoints(endpoints => {
     
                endpoints.MapControllers();
            });
        
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace galaxy_merchant_back
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
            services.AddRouting();
            services.AddCors();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {                
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;                
            });
            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());            
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc(configureRoutes: (routes) =>
            {
                routes.MapRoute(name: "Language", template: "{controller}/{action}/{id}", defaults: new { Controllers = "Language" });
            });
        }
    }
}

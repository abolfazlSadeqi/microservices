
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;




        }

        

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
                   .AddEnvironmentVariables()
                   .AddJsonFile("ocelot.json")
                   .Build());

            services.AddControllers();
            services.AddHttpContextAccessor();



            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddOcelot();



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            //app.UseHttpsRedirection();
            app.UseRouting();
         //   app.UseAuthorization();
         

            app.UseOcelot().Wait();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Configure the HTTP request pipeline.

        }
    }
}

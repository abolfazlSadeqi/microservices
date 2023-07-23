
using Application;
using Common;
using Common.HelperLog;
using Common.UI.Method;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace PublicApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }



    public void ConfigureServices(IServiceCollection services)
    {
        LoggerSerilogELK.ConfigureLogging(Configuration);

        services.AddApplication();
        services.AddInfrastructure(Configuration);

        services.AddSerilog();
        IdentityModelEventSource.ShowPII = true;    //show detail of error and see problem
        HelperBaseService.ConfigureService(services, Configuration);

        HelperAuthentication.ConfigureService(services, Configuration);


        #region oldCode
        //// prevent from mapping "sub" claim to nameidentifier.
        //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        //var identityUrl = Configuration.GetValue<string>("IdentityUrl");

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


        //}).AddJwtBearer(options =>
        //{
        //    options.Authority = identityUrl;
        //    options.RequireHttpsMetadata = false;
        //    options.Audience = "TransactionService";
        //    options.RequireHttpsMetadata = false;
        //    options.IncludeErrorDetails = true;
        //    options.SaveToken =true;
        //    options.TokenValidationParameters = new TokenValidationParameters()
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = false,

        //        ClockSkew = TimeSpan.Zero,
        //        RequireSignedTokens = true,

        //        ValidateIssuerSigningKey = true,

        //        RequireExpirationTime = true,
        //        ValidateLifetime = true,

        //        // ValidAudience = configuration["Jwt:Audience"],
        //        ValidIssuer = Configuration["Jwt:Issuer"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        //    };

        //});
        #endregion
        HelperSwagger.ConfigureService(services);
     
        //services.AddControllersWithViews(options =>
        //            options.Filters.Add<ApiExceptionFilterAttribute>())
        //                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);



    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
       

        // Seed Data
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var scopeProvider = scope.ServiceProvider;
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //  await ApplicationDbContextSeed.SeedSampleDataAsync(dbContext);
        }

       
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }





        app.UseStaticFiles();

        app.UseHttpsRedirection();


        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        


        //app.UseSwaggerUI(c =>
        //{
        //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DefaultCQRSAPI V1");
        //});

       // app.UseSerilogRequestLogging();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //app.Run();


        //  app.Run();

    }



}
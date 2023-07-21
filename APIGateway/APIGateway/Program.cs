
using APIGateway;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

            });




}



//using Ocelot.DependencyInjection;
//using Ocelot.Middleware;

//var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//// .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
////.AddEnvironmentVariables();
//builder.Services.AddOcelot();
//var app = builder.Build();

//app.UseRouting();
//app.UseOcelot();
//app.UseOcelot().Wait();

//app.UseEndpoints(endpoints => {
//    endpoints.MapGet("/", async context => {
//        await context.Response.WriteAsync("Hello World!");
//    });
//});

//app.Run();

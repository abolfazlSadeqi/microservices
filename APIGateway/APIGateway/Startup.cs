
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


namespace APIGateway;

public class Startup
{
    //public Startup(IConfiguration configuration)
    //{
    //    Configuration = configuration;

       

    //}

    public Startup(IConfiguration configuration, IWebHostEnvironment  environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment  Environment { get; }

   // public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {





        //var identityUrl = Configuration.GetValue<string>("IdentityUrl");
        //var authenticationProviderKey = "IdentityApiKey";
        ////…
        //services.AddAuthentication()
        //    .AddJwtBearer(authenticationProviderKey, x =>
        //    {
        //        x.Authority = identityUrl;
        //        x.RequireHttpsMetadata = false;
        //        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        //        {
        //            ValidAudiences = new[] { "TransactionService", "CustomerService" }
        //        };
        //    });
        //...

        //services.AddControllers();
        //services.AddHttpContextAccessor();



        //services.AddControllers();
        //services.AddEndpointsApiExplorer();

        // services.AddOcelot();

        //services.AddSwaggerForOcelot(Configuration);
        //  services.AddSwaggerGen();

        // services.AddOcelot(Configuration);

        // Swagger for ocelot

        //    services.AddSwaggerForOcelot(Configuration);




        //Configuration.AddOcelotWithSwaggerSupport(options =>
        //{
        //    options.Folder = routes;
        //});



        //  var environment = Environment.
        //builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        //    .AddOcelot(routes, builder.Environment)
        //    .AddEnvironmentVariables();

        // Add services to the container.



        services.AddOcelot(Configuration);//.AddPolly();
        services.AddSwaggerForOcelot(Configuration);


        var routes = "Routes";
        var _Directory = Directory.GetCurrentDirectory();
        services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
              .AddOcelotWithSwaggerSupport(options =>
              {
                  options.Folder = routes;
              }).SetBasePath(_Directory).AddOcelot(routes, null)//.AddEnvironmentVariables()
              .Build()
              );

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        // Swagger for ocelot
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {



        // Configure the HTTP request pipeline.
        
            app.UseSwagger();


        app.UseRouting();//error message suggested to implement this
        app.UseStaticFiles();//error message suggested to implement this

        app.UseHttpsRedirection();

        app.UseAuthorization();
        //app.UseOcelot().Wait();

        app.UseSwaggerForOcelotUI(options =>
        {
            options.PathToSwaggerGenerator = "/swagger/docs";
            options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

        }).UseOcelot().Wait();

        //   app.MapControllers();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        // Configure the HTTP request pipeline.

    }
}

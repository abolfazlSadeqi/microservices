using APIGateway;
using APIGateway.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var routes = "Routes";

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routes;
});
IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidateAudience = false,

            ClockSkew = TimeSpan.Zero,
            RequireSignedTokens = true,

            ValidateIssuerSigningKey = true,

            RequireExpirationTime = true,
            ValidateLifetime = true,

            // ValidAudience = configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });


//var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
//var authenticationProviderKey = builder.Configuration.GetValue<string>("IdentityApiKey");
////…
//builder.Services.AddAuthentication()
//    .AddJwtBearer(authenticationProviderKey, x =>
//    {
//        x.Authority = identityUrl;

//        x.RequireHttpsMetadata = false;
//        x.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = false,

//            ClockSkew = TimeSpan.Zero,
//            RequireSignedTokens = true,

//            ValidateIssuerSigningKey = true,

//            RequireExpirationTime = true,
//            ValidateLifetime = true,

//            // ValidAudience = configuration["Jwt:Audience"],
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        }; 
//    });
//// Map Okta scp to scope claims instead of http://schemas.microsoft.com/identity/claims/scope to allow ocelot to read/verify them
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("scp");
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("scp", "scope");


IdentityModelEventSource.ShowPII = true;


builder.Services.AddOcelot(builder.Configuration);//.AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 2147483647;
});
//builder.Services.AddJwtAuthentication();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger for ocelot
builder.Services.AddSwaggerGen();

var CustomOptions = new CustomRateLimitOptions();
builder.Configuration.GetSection(CustomRateLimitOptions.CustomRateLimit).Bind(CustomOptions);


builder.Services.AddRateLimiter(options =>
{


    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = CustomOptions.HttpStatusCode;
        context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter);

        await context.HttpContext.Response.WriteAsync($"+{Messages.RateLimited}" + (retryAfter.TotalMinutes > 0 ? "after " + retryAfter.TotalMinutes + " minutes " : "later"));

    };
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
RateLimitPartition.GetFixedWindowLimiter(
partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),

factory: partition => new FixedWindowRateLimiterOptions
{
    AutoReplenishment = CustomOptions.AutoReplenishment,
    PermitLimit = CustomOptions.PermitLimit,
    QueueLimit = CustomOptions.QueueLimit,
    Window = TimeSpan.FromMinutes(CustomOptions.Minute)
}));




});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
IdentityModelEventSource.ShowPII = true;



app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRateLimiter();
app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseAuthentication().UseOcelot().Wait();
IdentityModelEventSource.ShowPII = true;


app.MapControllers();

app.Run();

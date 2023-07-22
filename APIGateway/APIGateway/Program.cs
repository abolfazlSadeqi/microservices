using APIGateway;
using APIGateway.Config;
using Microsoft.Extensions.Configuration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var routes = "Routes";

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routes;
});

builder.Services.AddOcelot(builder.Configuration);//.AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();


// Add services to the container.

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


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRateLimiter();
app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseOcelot().Wait();

app.MapControllers();

app.Run();

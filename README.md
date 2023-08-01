This is simple application to microservice with swagger and jwt and ocelot that every projcts(1.api getway 2.authserver 3.two microservice) is CleanArchitecture with JWT and EF7 and .Net7 and RedisDistributedCaching and Logging with elasticsearch
## Projects microservice:
1.api getway 

2.authserver 

3.two microservice

## Ocelot

Ocelot is a .NET API Gateway.  Ocelot is aimed at people using .NET running a micro services / service orientated architecture that need a unified point of entry into their system.

#### Base Setps
1.Install Ocelot package on NuGet 

2.add Ocelot json Files and Custom Config

json Files

|FileName|Desc|
|--|---|
|ocelot.global.json	|Global Config|
|ocelot.SwaggerEndPoints|	Config  related Swagger|
|ocelot.[FileName].api	|Config  related Route ,Ocelot’s describes the routing of one request to another as a Route =It is better to create a file for each category or project ocelot	Orginal File|


ocelot.[FileName].api

|Title|Desc|	
|--|---|
|Downstream| a request sent by the Gateway to the microservice •DownstreamPathTemplate •DownstreamScheme•	SwaggerKey• DownstreamHostAndPorts|
|Upstream|request as referred to Ocelot is the client request as received by the API Gateway•	UpstreamPathTemplate•	UpstreamHttpMethod(post,get,put,…)|

https://github.com/abolfazlSadeqi/microservices/blob/master/APIGateway/APIGateway/Routes/ocelot.AuthServer.api.json

ocelot.global.json: GlobalConfiguration	Include Global Configuration(example urlbase)
 
ocelot.SwaggerEndPoints

|Title|Desc|	
|--|---|	
|Key	|Key ,The Key is used in the main file,Config	You must specify the configuration of the main Swagger(Name, Version, Url)|

3.add Configuration Ocelot in program file 
 a)Specifying the Ocelot folder 
 b)add basic settings and Swagger settings 
 c)creating the final file based on the folder

https://github.com/abolfazlSadeqi/microservices/blob/master/APIGateway/APIGateway/Routes/ocelot.SwaggerEndPoints.json


### Add Swagger
Setps

1.install pakcage
•	Swashbuckle.AspNetCore.Swagger
•	MMLib.SwaggerForOcelot

2.add key  SwaggerKey in OcelotFilecofnig(ocelot.[FileName].api)
```
"SwaggerKey": "AuthServer",
```

3.add Config SwaggerEndPoints in OcelotFilecofnig (ocelot.SwaggerEndPoints )
```
"SwaggerEndPoints": [
    {
      "Key": "transaction",
      "TransformByOcelotConfig": true,
      "Config": [
        {
          "Name": "Transaction.API",
          "Version": "1.0",
          "Url": "https://localhost:8035/swagger/v1/swagger.json"
        }
      ]
    }
```
4.add config Swagger and Ocelot(UseSwaggerForOcelotUI, UseSwagger,…)
```
app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseAuthentication().UseOcelot().Wait();
```


### Retry pattern 

Enable an application to handle transient failures when it tries to connect to a service or network resource, by transparently retrying a failed operation. This can improve the stability of the application.
Setps

1.install-package Polly
2.declare Variable AsyncRetryPolicy
 ```
private readonly AsyncRetryPolicy _retryPolicy;
```

3.add to controller(add policy  and Handle Exception)
 ```
_retryPolicy = Policy.Handle<Exception>().RetryAsync(2);
```

4.use in action
   ```
  var result = _retryPolicy.ExecuteAsync(() => Mediator.Send(new GetTranscationBaseQuery { Id = id }));
            return await result;
```




### Circuit-breaker

The Circuit Breaker pattern, popularized by Michael Nygard in his book, Release It!, can prevent an application from repeatedly trying to execute an operation that's likely to fail. Allowing it to continue without waiting for the fault to be fixed or wasting CPU cycles while it determines that the fault is long lasting. The Circuit Breaker pattern also enables an application to detect whether the fault has been resolved. If the problem appears to have been fixed, the application can try to invoke the operation.

Different states of circuit breaker
1.Close : When everything is normal, the circuit breakers remained closed, and all the request passes through to the services as shown below. If the number of failures increases beyond the threshold, the circuit breaker trips and goes into an open state.

2.Open state : In this state circuit breaker returns an error immediately without even invoking the services. The Circuit breakers move into the half-open state after a timeout period elapses. Usually, it will have a monitoring system where the timeout will be specified.

3.Half-open state: In this state, the circuit breaker allows a limited number of requests from the Microservice to passthrough and invoke the operation. If the requests are successful, then the circuit breaker will go to the closed state. However, if the requests continue to fail, then it goes back to Open state.
Setps
1.install-package Polly
2.declare Variable

   ```
private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
   ```

3.add to controller(add policy  and Handle Exception)
 ```
_circuitBreakerPolicy = Policy.Handle<Exception>()
                                            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
   ```
4.use in action
 ```
var result = _circuitBreakerPolicy.ExecuteAsync(() => Mediator.Send(query));
   ```
 
### Authentication and authorization in Ocelot 

1.install Packages in Nuget
•	System.IdentityModel.Tokens.Jwt
•	Microsoft.AspNetCore.Authentication.JwtBearer
2.Add AuthenticationOptions incude  ProviderKey and Other Options in Ocelot file
   ```
"AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer",
        "AllowedScopes": []
      }
   ```

3.add Config Jwt in appsetting in APIGateway and Client project
```
"Jwt": {
    "Key": "key",
    "Issuer": "https://localhost:5035/",
    "Audience": "Service",
    "Subject": "JWTServiceAccessToken"
  }

```
4.add Login(return Token) Action in AuthServer project 

5.add setting Authentication in client project
```
return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,

                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidAudience = configuration["Jwt:Audience"],
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
```

## Tech Specification:

1.authentication and authorization in  Web Api by using Identity and JWT
  
2.RedisDistributedCaching in  Web Api
  
3.Logging with Serilog and elasticsearch in  Web Api   
  
4.ReportBusinessObjectStimulSoft  in  Web Mvc
  
5.Read And Write Config  in  Web Mvc
  
6.TDD(XUnit) include 1.UnitTest 2.IntegrationTests
  
7.BDD (SpecFlow)
  
8.EFCore7 
  
9.Net7
  
10.Swagger UI
### Config Elasticsearch or Serilog or JWT or Identity or Clean Architecture

https://github.com/abolfazlSadeqi/ReportBusinessObjectStimulSoft/blob/master/README.md


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Shared.Configuration.Concrete;
using Shared.Configuration.Interface;
using PathComputationMicroService.Services.Concrete;
using PathComputationMicroService.Services.Interface;
using Shared.Authentication;
using static Shared.Authentication.JwtCookieAuthentication.JwtCookieAuthenticationHandler;
using static Shared.Authentication.JwtCookieAuthentication;
using Shared.Services.Concrete;
using Shared.Services.Interface;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var appConfig = new AppConfiguration(config);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();

    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "ApiKeyAuthentication";
    options.DefaultChallengeScheme = "ApiKeyAuthentication";
}).AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKeyAuthentication", options => { });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtCookieAuthentication";
    options.DefaultChallengeScheme = "JwtCookieAuthentication";
}).AddScheme<JwtCookieAuthenticationOptions, JwtCookieAuthenticationHandler>("JwtCookieAuthentication", options =>
{
    options.CookieName = "jwtToken";
});

builder.Services.AddTransient<IAppConfiguration>(_ => appConfig);
builder.Services.AddScoped<IPathComputationService, PathComputationService>();
builder.Services.AddScoped<IJwtTokenOpsServicecs, JwtTokenOpsServicecs>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

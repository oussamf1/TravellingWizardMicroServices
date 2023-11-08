using Microsoft.EntityFrameworkCore;
using Shared.Configuration.Concrete;
using Shared.Configuration.Interface;
using Shared.Data;
using UserOperationsMicroService.Services.Concrete;
using UserOperationsMicroService.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Services.Interface;
using Shared.Services.Concrete;
using Shared.Repos.Interface;
using Shared.Repos.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using static Shared.Authentication.JwtCookieAuthentication.JwtCookieAuthenticationHandler;
using static Shared.Authentication.JwtCookieAuthentication;

var builder = WebApplication.CreateBuilder(args);


IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var appConfig = new AppConfiguration(config);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(appConfig.DatabaseConnectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly("UserOperationsMicroService")));

builder.Services.AddTransient<IAppConfiguration>(_ => appConfig);
builder.Services.AddScoped<IUserRepo,UserRepo>();
builder.Services.AddScoped<IVacationPlanRepo,VacationPlanRepo>();
builder.Services.AddScoped<IEmailTemplateRepo, EmailTemplateRepo>();
builder.Services.AddScoped<IVacationPlanService,VacationPlanService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordComputationService,PasswordComputationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserActionsService, UserActionsService>();
builder.Services.AddScoped<IJwtTokenOpsServicecs, JwtTokenOpsServicecs>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientFront", builder =>
    {
        builder.WithOrigins("https://happy-moss-079f83603.4.azurestaticapps.net")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
builder.Services.AddAuthentication("JwtCookieAuthentication")
             .AddScheme<JwtCookieAuthenticationOptions, JwtCookieAuthenticationHandler>("JwtCookieAuthentication", options =>
             {
                options.CookieName = "jwtToken";                         
             });



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowClientFront");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

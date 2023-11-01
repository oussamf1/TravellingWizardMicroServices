using Microsoft.EntityFrameworkCore;
using UserOperationsMicroService.Configuration.Concrete;
using UserOperationsMicroService.Configuration.Interface;
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

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
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

builder.Services.AddScoped<IAppConfiguration>(_ => appConfig);
builder.Services.AddScoped<IUserRepo,UserRepo>();
builder.Services.AddScoped<IVacationPlanRepo,VacationPlanRepo>();
builder.Services.AddScoped<IVacationPlanService,VacationPlanService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordComputationService,PasswordComputationService>();
builder.Services.AddScoped<IJwtTokenOpsServicecs,JwtTokenOpsServicecs>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserActionsService, UserActionsService>();


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
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var jwtsecret = Encoding.ASCII.GetBytes(appConfig.JwtSecret);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtsecret),
        ValidateIssuer = false ,
        ValidateAudience = false ,
        ValidateLifetime = true ,
    };
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

app.UseCors("AllowLocalhost3000");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using BackgroundTasksMicroService;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using BackgroundTasksMicroService.Configuration.Concrete;
using BackgroundTasksMicroService.Configuration.Interface;
using Shared.Repos.Concrete;
using Shared.Repos.Interface;
using Shared.Services.Interface;
using Shared.Services.Concrete;
using Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        IConfigurationRoot config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();
        var appConfig = new AppConfiguration(config);

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(appConfig.DatabaseConnectionString);
        });
        services.AddSingleton<IAppConfiguration>(_ => appConfig);
        services.AddScoped<IVacationPlanService, VacationPlanService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IVacationPlanRepo,VacationPlanRepo>();
        services.AddScoped<IEmailTemplateRepo, EmailTemplateRepo>();
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IUserService, UserService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();


using BackgroundTasksMicroService.Configuration.Interface;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Services.Interface;
using System;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;

namespace BackgroundTasksMicroService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IAppConfiguration _appConfiguration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;


        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IAppConfiguration appConfig, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _appConfiguration = appConfig;
            _serviceProvider = serviceProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceProvider.CreateScope())
                {
                    var vacationPlanService = scope.ServiceProvider.GetRequiredService<IVacationPlanService>();

                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var plans = await vacationPlanService.GetNonGeneratedPlans();

                    var usersPlans = plans.Where(p => p.isGenerated == false).GroupBy(plan => plan.UserId);

                    foreach (var userPlans in usersPlans)
                    {
                        int userId = userPlans.Key;

                        var user = await userService.GetUserById(userId);

                        foreach (var plan in userPlans)
                        {
                            VacationPlanDTO vacationPlanDTO = JsonConvert.DeserializeObject<VacationPlanDTO>(plan.planInJson);

                            var generatedPlans = await GetPlans(vacationPlanDTO);

                            plan.GeneratedPlanInJson = JsonConvert.SerializeObject(generatedPlans);

                            plan.isGenerated= true;

                            await vacationPlanService.UpdatePlan(plan);

                            string emailContent = ConstructEmailContent(generatedPlans);

                            await emailService.SendSuggestedPlans(user, generatedPlans,emailContent,plan.SubmissionDate.ToString("dd/MM/yyyy"),_appConfiguration.FrontEndURl);
                        }

                    }

                }
                _hostApplicationLifetime.StopApplication();

                await Task.Delay(1000, stoppingToken);
            }
        }
        private async Task<IEnumerable<TripsPlan>> GetPlans(VacationPlanDTO vacationPlanDTO)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"{_appConfiguration.ComputationMicroserviceUrl}/Trips/getPlansForService";

                httpClient.DefaultRequestHeaders.Add("ApiKey", _appConfiguration.ApiKey); 

                HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, vacationPlanDTO);

                string content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<TripsPlan>>(content);

            }
        }
        private string ConstructEmailContent(IEnumerable<TripsPlan> plans)
        {
            StringBuilder emailContent = new StringBuilder();
            int tripCounter = 1;
            foreach(var plan in plans) 
            {
                emailContent.AppendLine($"<h2>Plan {tripCounter}: {plan.Price}€</h2>");
                foreach(var trip in plan.Trips)
                {
                    emailContent.AppendLine($"<text>{trip.Trip_starting_location.City} ({trip.Trip_starting_location.City_code}) --> ");
                }

                emailContent.AppendLine($"{plan.Trips.Last().Trip_ending_location.City} ({plan.Trips.Last().Trip_ending_location.City_code}) </text>");

            }
            return emailContent.ToString();
        }
    }
}
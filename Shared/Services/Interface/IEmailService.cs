using Shared.Models;

namespace Shared.Services.Interface
{
    public interface IEmailService
    {
        public Task SendConfirmationEmail(User user, string confirmationUrl);
        public Task SendSuggestedPlans(User user, IEnumerable<TripsPlan> plans, string emailContent, string submissionDate, string frontEndUrl);

    }
}

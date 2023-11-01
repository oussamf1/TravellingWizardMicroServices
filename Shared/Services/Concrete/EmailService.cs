using System.Net.Mail;
using System.Net;
using Shared.Services.Interface;
using Shared.Models;

namespace Shared.Services.Concrete
{
    public class EmailService : IEmailService
    {
        public void sendEmail( string recipient, string subject, string body)
        {

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("oussamafathallah854@gmail.com", "rlnf shqt kbtd ebld"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("oussamafathallah854@gmail.com"),
                Subject = subject,
                Body = body,  // Set the HTML content here
                IsBodyHtml = true, 
            };

            mailMessage.To.Add(recipient);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

        }
        private string GetEmailSubject(EmailType emailType)
        {
            switch (emailType)
            {
                case EmailType.Confirmation:
                    return "Email Confirmation";
                case EmailType.Registration:
                    return "Welcome to Our Service";
                case EmailType.SuggestPlans:
                    return "Vacation plans by Travelling Wizard";

                default:
                    return "Default Email Body";
            }
        }

        public async Task SendConfirmationEmail(User user, string confirmationUrl)
        {
            string subject = GetEmailSubject(EmailType.Confirmation);
            string currentDirectory = Directory.GetCurrentDirectory();
            string templateFolderPath = Path.Combine(currentDirectory, "EmailTemplates");
            string templateFileName = "confirmation-email.html"; 

            string templateFilePath = Path.Combine(templateFolderPath, templateFileName);

            string emailTemplate = File.ReadAllText(templateFilePath);
            emailTemplate = emailTemplate.Replace("{email}", user.Email);
            emailTemplate = File.ReadAllText(templateFilePath);
            emailTemplate = emailTemplate.Replace("{confirmationLink}", confirmationUrl);

            sendEmail(user.Email, subject, emailTemplate);
        }

        public async Task SendSuggestedPlans(User user, IEnumerable<TripsPlan> plans, string emailContent, string submissionDate, string frontEndUrl)
        {
            string subject = GetEmailSubject(EmailType.Confirmation);
            string currentDirectory = Directory.GetCurrentDirectory();
            string templateFolderPath = Path.Combine(currentDirectory, "EmailTemplates");
            string templateFileName = "suggested-plans-email.html";
            string templateFilePath = Path.Combine(templateFolderPath, templateFileName);
            string emailTemplate = File.ReadAllText(templateFilePath);
            string fullName = user.FirstName + " " + user.LastName;
            emailTemplate = emailTemplate.Replace("{SubmissionDate}", submissionDate);
            emailTemplate = emailTemplate.Replace("{plansContentInHTML}", emailContent);
            emailTemplate = emailTemplate.Replace("{FullName}", fullName);
            emailTemplate = emailTemplate.Replace("{frontEndUrl}", frontEndUrl);
            sendEmail(user.Email, subject, emailTemplate);
        }
    }
}

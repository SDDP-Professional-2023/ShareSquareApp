using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ShareSquareApp.Services
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetOptions _mailJetOptions;

        public MailJetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Get MailJet options from the configuration
            _mailJetOptions = _configuration.GetSection("MailJet").Get<MailJetOptions>();

            // Create a new MailJet client with the API key and Secret Key
            MailjetClient client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey);

            // Create  a new MailJet request for sending emails 
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            //Set the properties of the email like recipients, subject and body
               .Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", "dotnetProfessionals@protonmail.com"},
                  {"Name", "dotnetProfessionals"}
                  }},
                 {"To", new JArray {
                  new JObject {
                   {"Email", email},
                   {"Name", "receiver"}
                   }
                  }},
                 {"Subject", subject},
                 {"HTMLPart", htmlMessage}
                 }
                   });

            // send the email asynchronously
            await client.PostAsync(request);
        }
    }
}

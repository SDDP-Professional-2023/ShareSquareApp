using MimeKit;
using MailKit.Net.Smtp;
using ShareSquareApp.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ShareSquare.Data.Models;

namespace ShareSquareApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailServiceOptions _emailServiceOptions;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                //_emailServiceOptions = _configuration.GetSection("EmailServices").Get<EmailServiceOptions>();
                //var message = new MimeMessage();
                //message.From.Add(new MailboxAddress("ShareSquare", _emailServiceOptions.FromEmail));
                //message.To.Add(new MailboxAddress("Test", "c2069691@my.shu.ac.uk"));
                //message.Subject = subject;

                //message.Body = new TextPart("html")
                //{
                //    Text = body
                //};

                //using (var client = new MailKit.Net.Smtp.SmtpClient())
                //{
                //    client.Connect("smtp.gmail.com", 587, false);

                //    client.Authenticate(_emailServiceOptions.AuthenticateEmail, _emailServiceOptions.AuthenticatePassword);

                //    client.Send(message);
                //    client.Disconnect(true);
                //}
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"Error trying to send: {ex.Message}");
                Console.WriteLine($"StatusCode: {ex.StatusCode}");
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"Protocol error while trying to send: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while trying to send: {ex.Message}");
            }
        }
    }
}

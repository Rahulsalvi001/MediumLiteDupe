using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            _options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sendGridOptions = new SendGridClientOptions
            {
                ApiKey = _options.ApiKey
            };
            var emailClient = new SendGridClient(sendGridOptions);
            var message = new SendGridMessage
            {
                From = new EmailAddress("rajswnt7@gmail.com"),
                Subject = subject,
                HtmlContent = htmlMessage
            };           
            
            message.AddTo("rahulsalvi001@gmail.com");
            var resp = emailClient.SendEmailAsync(message);
            var result = resp.GetAwaiter().GetResult();
            return resp;

            //MailMessage msg = new MailMessage();
            //msg.To.Add(email);
            //msg.From = new MailAddress("rajswnt7@gmail.com");
            //msg.Subject = subject;
            //msg.Body = htmlMessage;
            //msg.IsBodyHtml = true;

            //SmtpClient client = new SmtpClient("smtp.gmail.com");
            //client.Port = 587;
            //client.UseDefaultCredentials = true;
            //client.EnableSsl = false;
            //client.Credentials = new System.Net.NetworkCredential("rajswnt7@gmail.com", "---");
            //return client.SendMailAsync(msg);
        }
        

    }
}

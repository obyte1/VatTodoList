using MimeKit;
using static VAT_TODOLIST.Services.EmailServices;
//using System.Net.Mail;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using VAT_TODOLIST.Interface;
using VAT_TODOLIST.Models;


namespace VAT_TODOLIST.Services
{
    public class EmailServices
    {
        public class EmailService : IEmailService
        {
            private readonly EmailServerConfig _emailConfig;
            public EmailService(EmailServerConfig emailConfig)
            {
                _emailConfig = emailConfig;
            }
            public async Task SendEmailAsync(EmailMessageContent message)
            {
                var emailMessage = CreateEmailMessage(message);
                await SendAsync(emailMessage);
            }

            private MimeMessage CreateEmailMessage(EmailMessageContent message)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("VATINTERNS TODO ALERT", _emailConfig.From));
                emailMessage.To.AddRange(message.To);
                emailMessage.Subject = message.Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<p>{0}</p>", message.Content) };
                return emailMessage;
            }
            private async Task SendAsync(MimeMessage mailMessage)
            {
                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                        await client.SendAsync(mailMessage);
                    }
                    catch
                    {
                        //log an error message or throw an exception or both.
                        throw;
                    }
                    finally
                    {
                        client.Disconnect(true);
                        client.Dispose();
                    }
                }
            }
        }
    }
}

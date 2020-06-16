using System.Net;
using System.Net.Mail;

namespace LightTown.Server.Services.Mail
{
    public class SmtpMailService : IMailService
    {
        public void SendEmail(string email, string subject, string body)
        {
            SmtpClient client = new SmtpClient(Config.Config.SmtpServerHost)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Config.Config.SmtpServerUsername, Config.Config.SmtpServerPassword),
                Port = Config.Config.SmtpServerPort,
                EnableSsl = Config.Config.SmtpServerEnableSsl
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(Config.Config.SmtpServerEmail),
                Body = body,
                Subject = subject,
                To = { email }
            };

            client.Send(mailMessage);
        }
    }
}
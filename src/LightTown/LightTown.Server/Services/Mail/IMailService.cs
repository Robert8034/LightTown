namespace LightTown.Server.Services.Mail
{
    public interface IMailService
    {
        void SendEmail(string email, string subject, string body);
    }
}
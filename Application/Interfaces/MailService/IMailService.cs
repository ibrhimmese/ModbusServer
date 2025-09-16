namespace Application.Interfaces.MailService;

public interface IMailService
{
    Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true);
    Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true);

    Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
}

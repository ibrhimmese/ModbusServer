using Application.Interfaces.MailService;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerce.Infrastructure.MailServiceConcrete;

public class MailService(IConfiguration configuration) : IMailService
{
    public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
       await SendMessageAsync(new[] {to}, subject, body, isBodyHtml);
    }

    public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
    {
        MailMessage mail = new();
        mail.IsBodyHtml = isBodyHtml;
        foreach (var to in tos)
        {
            mail.To.Add(to);
        }
        mail.Subject = subject;
        mail.Body = body;
        mail.From = new(configuration["MailService:Username"]!,"İBRAHİM MEŞE",System.Text.Encoding.UTF8);

        SmtpClient smtp = new SmtpClient();
        smtp.Credentials = new NetworkCredential(configuration["MailService:Username"], configuration["MailService:Password"]);
        smtp.Port = Convert.ToInt32(configuration["MailService:Port"]);
        smtp.EnableSsl = true;
        smtp.Host = configuration["MailService:Host"]!;
        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        StringBuilder mail = new();
        mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebiliriniz.<br><strong><a target=\"_blank\" href=\"");
        mail.AppendLine(configuration["ClientUrl"]);
        mail.AppendLine("/update-password/");
        mail.AppendLine(userId);
        mail.AppendLine("/");
        mail.AppendLine(resetToken);
        mail.AppendLine("\">Yeni Şifre Talebi için tıklayınız</a></strong><br><br><span style=\"font-size:12px;\">Not: Eğer bu talep tarafınızca gerçekleştirilmişse bu maili ciddiye almayın.</span><br>Saygılarımızla...<br><br><br> E-Ticaret");

        await SendMessageAsync(to, "Şifre Yenileme Talebi", mail.ToString());
       
    }
}

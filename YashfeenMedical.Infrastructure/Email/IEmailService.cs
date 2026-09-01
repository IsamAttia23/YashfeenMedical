namespace YashfeenMedical.Infrastructure.Email;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody);
}

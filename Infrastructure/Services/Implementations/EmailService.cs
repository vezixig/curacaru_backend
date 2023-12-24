namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

internal class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public void SendPasswordMail(string email, string password)
    {
        using var client = new SmtpClient(Environment.GetEnvironmentVariable("EMAIL_SMTP"), 587);
        logger.LogInformation($"SMTP - Port: {client.Port}");
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("EMAIL_USER"), Environment.GetEnvironmentVariable("EMAIL_PASSWORD"));
        client.EnableSsl = true;

        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(Environment.GetEnvironmentVariable("EMAIL_USER"));
        mailMessage.To.Add(email);
        mailMessage.Subject = "Curacaru - Dein Passwort";
        mailMessage.Body = $"Dein Passwort lautet: {password}";

        client.Send(mailMessage);
    }
}
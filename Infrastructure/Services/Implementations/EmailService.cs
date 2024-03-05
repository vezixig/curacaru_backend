namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using System.Net;
using System.Net.Mail;

internal class EmailService : IEmailService
{
    public void SendPasswordMail(string email, string password)
    {
        var smtp = Environment.GetEnvironmentVariable("EMAIL_SMTP");
        if (string.IsNullOrEmpty(smtp)) throw new InvalidOperationException("Environment variable EMAIL_SMTP missing");

        var mailUser = Environment.GetEnvironmentVariable("EMAIL_USER");
        if (string.IsNullOrEmpty(mailUser)) throw new InvalidOperationException("Environment variable EMAIL_USER missing");

        var mailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        if (string.IsNullOrEmpty(mailPassword)) throw new InvalidOperationException("Environment variable EMAIL_PASSWORD missing");

        using var client = new SmtpClient(smtp, GetPort());
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(mailUser, mailPassword);
        client.EnableSsl = true;

        var mailMessage = new MailMessage();
        mailMessage.From = new(mailUser);
        mailMessage.To.Add(email);
        mailMessage.Subject = "Curacaru - Deine Zugangsdaten";
        mailMessage.Body =
            $"Hallo,\r\n\r\nsoeben wurde ein Zugang für dich bei Curacaru registriert.\r\n\r\nDeine Zugangsdaten lauten wie folgt:\r\nBenutzername: Deine E-Mail-Adresse\r\nPasswort: {password}";

        client.Send(mailMessage);
    }

    private static int GetPort()
    {
        var port = Environment.GetEnvironmentVariable("EMAIL_PORT");
        if (!string.IsNullOrEmpty(port) && int.TryParse(port, out var result)) return result;

        throw new InvalidOperationException("Environment variable EMAIL_PORT missing or invalid");
    }
}
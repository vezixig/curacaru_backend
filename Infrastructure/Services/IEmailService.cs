namespace Curacaru.Backend.Infrastructure.Services;

public interface IEmailService
{
    public void SendPasswordMail(string email, string password);
}
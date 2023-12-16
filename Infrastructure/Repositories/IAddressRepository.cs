namespace Curacaru.Backend.Infrastructure.Repositories;

public interface IAddressRepository
{
    Task<string?> GetCityAsync(string zipCode);
}
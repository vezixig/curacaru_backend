namespace Curacaru.Backend.Application.CQRS.Address;

using Infrastructure.Repositories;
using MediatR;

public class CityByZipCodeRequest : IRequest<string?>
{
    public CityByZipCodeRequest(string zipCode)
        => ZipCode = zipCode;

    public string ZipCode { get; }
}

public class CityByZipCodeHandler : IRequestHandler<CityByZipCodeRequest, string?>
{
    private readonly IAddressRepository _addressRepository;

    public CityByZipCodeHandler(IAddressRepository addressRepository)
        => _addressRepository = addressRepository;

    public async Task<string?> Handle(CityByZipCodeRequest request, CancellationToken cancellationToken)
        => await _addressRepository.GetCityAsync(request.ZipCode);
}
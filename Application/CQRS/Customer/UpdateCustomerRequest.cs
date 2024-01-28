namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class UpdateCustomerRequest(UpdateCustomerDto customerData, Guid companyId, string authId) : IRequest<GetCustomerDto>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public UpdateCustomerDto CustomerData { get; } = customerData;
}

public class UpdateCustomerRequestHandler(
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IMapper mapper,
    IMapService mapService)
    : IRequestHandler<UpdateCustomerRequest, GetCustomerDto>
{
    public async Task<GetCustomerDto> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerData.Id)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");

        if (customer.CompanyId != request.CompanyId) throw new ForbiddenException("Sie dürfen diesen Kunden nicht bearbeiten.");

        if (request.CustomerData.AssociatedEmployeeId.HasValue)
            _ = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.CustomerData.AssociatedEmployeeId.Value)
                ?? throw new BadRequestException("Bearbeitenden Mitarbeiter nicht gefunden.");

        // check if address has changed to update geolocation
        if (customer.Street != request.CustomerData.Street || customer.ZipCode != request.CustomerData.ZipCode)
        {
            var geolocation = await mapService.GetGeolocationAsync(request.CustomerData.Street, request.CustomerData.ZipCode);
            customer.Latitude = geolocation.Latitude;
            customer.Longitude = geolocation.Longitude;
        }

        mapper.Map(request.CustomerData, customer);
        customer.AssociatedEmployee = new Employee { Id = request.CustomerData.AssociatedEmployeeId!.Value };
        customer.Insurance = request.CustomerData.InsuranceId.HasValue ? new Insurance { Id = request.CustomerData.InsuranceId.Value } : null;
        customer.ZipCity = new ZipCity { ZipCode = request.CustomerData.ZipCode! };

        var updatedCustomer = await customerRepository.UpdateCustomerAsync(customer);
        return mapper.Map<GetCustomerDto>(updatedCustomer);
    }
}
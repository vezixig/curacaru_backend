namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class UpdateCustomerRequest(UpdateCustomerDto customerData, Guid companyId, string authId) : IRequest<GetCustomerDto>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public UpdateCustomerDto CustomerData { get; } = customerData;
}

public class UpdateCustomerRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
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

        customer.AssociatedEmployee = new Employee { Id = request.CustomerData.AssociatedEmployeeId!.Value };
        customer.AssociatedEmployeeId = request.CustomerData.AssociatedEmployeeId;
        customer.BirthDate = request.CustomerData.BirthDate;
        customer.CareLevel = request.CustomerData.CareLevel;
        customer.DeclarationsOfAssignment = request.CustomerData.DeclarationsOfAssignment;
        customer.DoClearanceCareBenefit = request.CustomerData.DoClearanceCareBenefit;
        customer.DoClearanceReliefAmount = request.CustomerData.DoClearanceReliefAmount;
        customer.EmergencyContactName = request.CustomerData.EmergencyContactName;
        customer.EmergencyContactPhone = request.CustomerData.EmergencyContactPhone;
        customer.FirstName = request.CustomerData.FirstName;
        customer.Insurance = request.CustomerData.InsuranceId.HasValue ? new Insurance { Id = request.CustomerData.InsuranceId.Value } : null;
        customer.InsuranceId = request.CustomerData.InsuranceId;
        customer.InsuranceStatus = request.CustomerData.InsuranceStatus;
        customer.InsuranceStatus = request.CustomerData.InsuranceStatus;
        customer.InsuredPersonNumber = request.CustomerData.InsuredPersonNumber;
        customer.IsCareContractAvailable = request.CustomerData.IsCareContractAvailable;
        customer.LastName = request.CustomerData.LastName;
        customer.Phone = request.CustomerData.Phone;
        customer.Street = request.CustomerData.Street;
        customer.ZipCity = new ZipCity { ZipCode = request.CustomerData.ZipCode! };
        customer.ZipCode = request.CustomerData.ZipCode;

        var updatedCustomer = await customerRepository.UpdateCustomerAsync(customer);
        return mapper.Map<GetCustomerDto>(updatedCustomer);
    }
}
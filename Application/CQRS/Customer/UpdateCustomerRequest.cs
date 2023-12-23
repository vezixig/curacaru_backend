namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class UpdateCustomerRequest : IRequest<GetCustomerDto>
{
    public UpdateCustomerRequest(UpdateCustomerDto customerData, Guid companyId, string authId)
    {
        CustomerData = customerData;
        CompanyId = companyId;
        AuthId = authId;
    }

    public string AuthId { get; }

    public Guid CompanyId { get; }

    public UpdateCustomerDto CustomerData { get; }
}

public class UpdateCustomerRequestHandler : IRequestHandler<UpdateCustomerRequest, GetCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IEmployeeRepository _employeeRepository;

    private readonly IMapper _mapper;

    public UpdateCustomerRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerDto> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerData.Id)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");
        if (customer.CompanyId != request.CompanyId) throw new ForbiddenException("Sie dürfen diesen Kunden nicht bearbeiten.");

        if (request.CustomerData.AssociatedEmployeeId.HasValue)
            _ = await _employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.CustomerData.AssociatedEmployeeId.Value)
                ?? throw new BadRequestException("Bearbeitenden Mitarbeiter nicht gefunden.");

        customer.AssociatedEmployeeId = request.CustomerData.AssociatedEmployeeId;
        customer.AssociatedEmployee = new Employee { Id = request.CustomerData.AssociatedEmployeeId!.Value };
        customer.BirthDate = request.CustomerData.BirthDate;
        customer.CareLevel = request.CustomerData.CareLevel;
        customer.DeclarationsOfAssignment = request.CustomerData.DeclarationsOfAssignment;
        customer.EmergencyContactName = request.CustomerData.EmergencyContactName;
        customer.EmergencyContactPhone = request.CustomerData.EmergencyContactPhone;
        customer.FirstName = request.CustomerData.FirstName;
        customer.InsuranceId = request.CustomerData.InsuranceId;
        customer.Insurance = request.CustomerData.InsuranceId.HasValue ? new Insurance { Id = request.CustomerData.InsuranceId.Value } : null;
        customer.InsuranceStatus = request.CustomerData.InsuranceStatus;
        customer.InsuredPersonNumber = request.CustomerData.InsuredPersonNumber;
        customer.InsuranceStatus = request.CustomerData.InsuranceStatus;
        customer.IsCareContractAvailable = request.CustomerData.IsCareContractAvailable;
        customer.LastName = request.CustomerData.LastName;
        customer.Phone = request.CustomerData.Phone;
        customer.Street = request.CustomerData.Street;
        customer.ZipCode = request.CustomerData.ZipCode;
        customer.ZipCity = new ZipCity { ZipCode = request.CustomerData.ZipCode! };

        var updatedCustomer = await _customerRepository.UpdateCustomerAsync(customer);
        return _mapper.Map<GetCustomerDto>(updatedCustomer);
    }
}
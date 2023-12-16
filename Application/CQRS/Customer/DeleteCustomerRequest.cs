namespace Curacaru.Backend.Application.CQRS.Customer;

using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class DeleteCustomerRequest : IRequest
{
    public DeleteCustomerRequest(string authId, Guid customerId)
    {
        AuthId = authId;
        CustomerId = customerId;
    }

    public string AuthId { get; }

    public Guid CustomerId { get; }
}

internal class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerRequest>
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IEmployeeRepository _employeeRepository;

    public DeleteCustomerRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
    {
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var user = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.IsManager == false) throw new ForbiddenException("Nur Manager dürfen Mitarbeiter löschen.");
        if (user.CompanyId == null) throw new ForbiddenException("Benutzer gehört zu keinem Unternehmen.");

        var customer = await _customerRepository.GetCustomerAsync(user.CompanyId.Value, request.CustomerId)
                       ?? throw new NotFoundException("Kunde nicht gefunden.");

        await _customerRepository.DeleteCustomerAsync(customer);
    }
}
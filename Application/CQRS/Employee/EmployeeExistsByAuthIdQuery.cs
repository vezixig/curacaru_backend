namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Entities;
using Infrastructure.repositories;
using MediatR;

public class EmployeeExistsByAuthIdQuery : IRequest<Employee?>
{
    /// <summary>Initializes a new instance of the <see cref="EmployeeExistsByAuthIdQuery" /> class.</summary>
    /// <param name="employeeId">The employee identifier.</param>
    public EmployeeExistsByAuthIdQuery(string employeeId)
        => EmployeeId = employeeId;

    public string EmployeeId { get; }
}

public class GetEmployeeByAuthIdQueryHandler : IRequestHandler<EmployeeExistsByAuthIdQuery, Employee?>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByAuthIdQueryHandler(IEmployeeRepository employeeRepository)
        => _employeeRepository = employeeRepository;

    public Task<Employee?> Handle(EmployeeExistsByAuthIdQuery request, CancellationToken cancellationToken)
        => _employeeRepository.GetEmployeeByAuthId(request.EmployeeId);
}
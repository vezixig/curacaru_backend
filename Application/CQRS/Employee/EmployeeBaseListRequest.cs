namespace Curacaru.Backend.Application.CQRS.Employee;

using AutoMapper;
using Core.DTO;
using Infrastructure.Repositories;
using MediatR;

public class EmployeeBaseListRequest(Guid companyId) : IRequest<IEnumerable<GetEmployeeBase>>
{
    public Guid CompanyId { get; } = companyId;
}

internal class EmployeeBaseListRequestHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<EmployeeBaseListRequest, IEnumerable<GetEmployeeBase>>
{
    public async Task<IEnumerable<GetEmployeeBase>> Handle(EmployeeBaseListRequest request, CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetEmployeesAsync(request.CompanyId);
        return mapper.Map<IEnumerable<GetEmployeeBase>>(employees);
    }
}
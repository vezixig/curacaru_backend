namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Query for getting all employees of the company the user is in.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="page">The page number.</param>
/// <param name="pageSize">The page size.</param>
public class EmployeeListRequest(Guid companyId, int page, int pageSize) : IRequest<PageDto<Employee>>
{
    public Guid CompanyId { get; } = companyId;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;
}

internal class EmployeeListQueryHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<EmployeeListRequest, PageDto<Employee>>
{
    public async Task<PageDto<Employee>> Handle(EmployeeListRequest request, CancellationToken cancellationToken)
    {
        double employeeCount = await employeeRepository.GetEmployeesCountAsync(request.CompanyId);
        var employees = await employeeRepository.GetEmployeesAsync(request.CompanyId, request.Page, request.PageSize);
        var pageCount = (int)Math.Ceiling(employeeCount / request.PageSize);
        return new(employees, request.Page, pageCount);
    }
}
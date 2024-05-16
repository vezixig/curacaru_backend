namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO;
using Core.DTO.Customer;
using Core.Enums;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for the customer list.</summary>
/// <param name="user">The authorized user.</param>
public class CustomerListRequest(
    User user,
    int page,
    int pageSize,
    Guid? employeeId,
    bool onlyActive) : IRequest<PageDto<GetCustomerListEntryDto>>
{
    public Guid? EmployeeId { get; } = employeeId;

    /// <summary>Gets a value indicating whether to only return active customers.</summary>
    public bool OnlyActive { get; set; } = onlyActive;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

public class CustomerListRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<CustomerListRequest, PageDto<GetCustomerListEntryDto>>
{
    public async Task<PageDto<GetCustomerListEntryDto>> Handle(CustomerListRequest request, CancellationToken cancellationToken)
    {
        var customerCount = await customerRepository.GetCustomerCountAsync(
            request.User.CompanyId,
            request.User.IsManager ? request.EmployeeId : request.User.EmployeeId,
            request.OnlyActive);

        var customers = await customerRepository.GetCustomersAsync(
            request.User.CompanyId,
            request.User.IsManager ? request.EmployeeId : request.User.EmployeeId,
            status: request.OnlyActive ? CustomerStatus.Customer : null,
            page: request.Page,
            pageSize: request.PageSize);

        var pageCount = (int)Math.Ceiling((decimal)customerCount / request.PageSize);
        return new(mapper.Map<List<GetCustomerListEntryDto>>(customers), request.Page, pageCount);
    }
}
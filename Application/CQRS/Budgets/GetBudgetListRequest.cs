namespace Curacaru.Backend.Application.CQRS.Budgets;

using AutoMapper;
using Core.DTO;
using Core.DTO.Budget;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to get the list of current budgets for a company.</summary>
public class GetBudgetListRequest(Guid companyId, int page, int pageSize) : IRequest<PageDto<GetBudgetListEntryDto>>
{
    public Guid CompanyId { get; } = companyId;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;
}

internal class GetBudgetListRequestHandler(
    IBudgetRepository budgetRepository,
    ICompanyRepository companyRepository,
    ICustomerRepository customerRepository,
    IMapper mapper)
    : IRequestHandler<GetBudgetListRequest, PageDto<GetBudgetListEntryDto>>
{
    public async Task<PageDto<GetBudgetListEntryDto>> Handle(GetBudgetListRequest request, CancellationToken cancellationToken)
    {
        var budgets = await budgetRepository.GetBudgetListAsync(request.CompanyId);
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);

        // get missing customers
        var customers = await customerRepository.GetCustomersAsync(request.CompanyId);
        customers.RemoveAll(customer => budgets.Exists(budget => budget.CustomerId == customer.Id));

        var result = mapper.Map<List<GetBudgetListEntryDto>>(budgets);
        result.AddRange(customers.Select(o => new GetBudgetListEntryDto { CustomerId = o.Id, CustomerName = $"{o.LastName}, {o.FirstName}".Trim() }));
        var pricePerHour = company!.PricePerHour == 0 ? 1 : company.PricePerHour;
        result.ForEach(o => o.RemainingHours = o.TotalAmount / pricePerHour);
        return new(
            result.OrderBy(o => o.CustomerName).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList(),
            request.Page,
            (int)Math.Ceiling((double)result.Count / request.PageSize));
    }
}
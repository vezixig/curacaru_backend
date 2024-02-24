namespace Curacaru.Backend.Application.CQRS.Budgets;

using AutoMapper;
using Core.DTO.Budget;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to get the list of current budgets for a company.</summary>
public class GetBudgetListRequest(Guid companyId) : IRequest<List<GetBudgetListEntryDto>>
{
    public Guid CompanyId { get; } = companyId;
}

internal class GetBudgetListRequestHandler(
    IBudgetRepository budgetRepository,
    ICompanyRepository companyRepository,
    ICustomerRepository customerRepository,
    IMapper mapper)
    : IRequestHandler<GetBudgetListRequest, List<GetBudgetListEntryDto>>
{
    public async Task<List<GetBudgetListEntryDto>> Handle(GetBudgetListRequest request, CancellationToken cancellationToken)
    {
        var budgets = await budgetRepository.GetBudgetListAsync(request.CompanyId);
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);

        // get missing customers
        var customers = await customerRepository.GetCustomersAsync(request.CompanyId);
        customers.RemoveAll(customer => budgets.Exists(budget => budget.CustomerId == customer.Id));

        var result = mapper.Map<List<GetBudgetListEntryDto>>(budgets);
        result.AddRange(customers.Select(o => new GetBudgetListEntryDto { CustomerId = o.Id, CustomerName = $"{o.FirstName} {o.LastName}".Trim() }));
        result.ForEach(o => o.RemainingHours = o.TotalAmount / company!.PricePerHour);
        return result;
    }
}
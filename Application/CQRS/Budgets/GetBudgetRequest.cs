namespace Curacaru.Backend.Application.CQRS.Budgets;

using Core.DTO.Budget;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;
using Services.Implementations;

/// <summary>Request to get the current budget for a customer.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="customerId">The customer id.</param>
public class GetBudgetRequest(Guid companyId, Guid customerId) : IRequest<GetBudgetDto>
{
    public Guid CompanyId { get; } = companyId;

    public Guid CustomerId { get; } = customerId;
}

public class GetBudgetRequestHandler(IBudgetRepository budgetRepository, ICompanyRepository companyRepository, ICustomerRepository customerRepository)
    : IRequestHandler<GetBudgetRequest, GetBudgetDto>
{
    public async Task<GetBudgetDto> Handle(GetBudgetRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerId)
                       ?? throw new BadRequestException("Kunde wurde nicht gefunden");

        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);

        var budget = await budgetRepository.GetCurrentBudgetAsync(request.CompanyId, request.CustomerId) ?? new Budget { Customer = new() };

        GetBudgetDto result = new()
        {
            CareBenefitAmount = budget.CareBenefitAmount,
            CareBenefitRaise = BudgetService.GetCareBenefitByCareLevel(customer.CareLevel),
            CustomerId = customer.Id,
            CustomerName = $"{customer.LastName}, {customer.FirstName}".Trim(),
            DoClearanceCareBenefit = customer.DoClearanceCareBenefit,
            DoClearancePreventiveCare = customer.DoClearancePreventiveCare,
            DoClearanceReliefAmount = customer.DoClearanceReliefAmount,
            DoClearanceSelfPayment = customer.DoClearanceSelfPayment,
            Id = budget.Id,
            PreventiveCareAmount = budget.PreventiveCareAmount,
            PricePerHour = company!.PricePerHour,
            ReliefAmount = budget.ReliefAmount + budget.ReliefAmountLastYear,
            ReliefAmountCurrentYear = budget.ReliefAmount,
            ReliefAmountPreviousYear = budget.ReliefAmountLastYear,
            ReliefAmountRaise = BudgetService.MonthlyReliefAmountRaise,
            PreventiveCareRaise = BudgetService.YearlyPreventiveCareRaise,
            SelfPayAmount = budget.SelfPayAmount,
            SelfPayRaise = budget.SelfPayRaise
        };

        return result;
    }
}
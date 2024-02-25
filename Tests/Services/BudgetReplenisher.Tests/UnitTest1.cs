namespace BudgetReplenisher.Tests;

using AutoFixture;
using Curacaru.Backend.Core.Entities;
using Curacaru.Backend.Infrastructure.Repositories;
using NSubstitute;

public class UnitTest1
{
    private readonly IBudgetRepository _budgetRepository = Substitute.For<IBudgetRepository>();

    private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();

    private readonly IFixture _fixture = new Fixture();

    private readonly Worker _sut;

    public UnitTest1()
        => _sut = new Worker(_customerRepository, _budgetRepository);

    [Fact]
    public async Task DoWorkAsync()
    {
        // Arrange
        var customers = _fixture.CreateMany<Customer>().ToList();
        var budgets = customers.Select(
                customer => new Budget
                {
                    CustomerId = customer.Id,
                    CareBenefitAmount = 0,
                    PreventiveCareAmount = 0,
                    ReliefAmount = 0,
                    SelfPayAmount = 0,
                    SelfPayRaise = 0,
                    Customer = customer,
                    CompanyId = customer.CompanyId
                })
            .ToList();

        _customerRepository.GetAllCustomersAsync().Returns(customers);
        _budgetRepository.GetAllBudgetsAsync().Returns(budgets);

        // Act
        await _sut.DoWorkAsync();

        // Assert
        await _customerRepository.Received(1).GetAllCustomersAsync();
        await _budgetRepository.Received(1).GetAllBudgetsAsync();
    }
}
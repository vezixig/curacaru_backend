namespace Application.Tests;

using Curacaru.Backend.Application.CQRS.Appointments;
using Curacaru.Backend.Application.Services;
using Curacaru.Backend.Core.Entities;
using Curacaru.Backend.Core.Enums;
using Curacaru.Backend.Infrastructure.repositories;
using Curacaru.Backend.Infrastructure.Repositories;
using Curacaru.Backend.Infrastructure.Services;
using FluentAssertions;
using NSubstitute;

public class DeleteAppointmentRequestTests
{
    private readonly IAppointmentRepository _appointmentRepository = Substitute.For<IAppointmentRepository>();

    private readonly IBudgetRepository _budgetRepository = Substitute.For<IBudgetRepository>();

    private readonly IDatabaseService _databaseService = Substitute.For<IDatabaseService>();

    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();

    private readonly IEmployeeRepository _employeeRepository = Substitute.For<IEmployeeRepository>();

    private readonly DeleteAppointmentRequestHandler _sut;

    public DeleteAppointmentRequestTests()
        => _sut = new(_appointmentRepository, _budgetRepository, _databaseService, _dateTimeService, _employeeRepository);

    // refund appointment form last year

    /// <summary>Tests the refunding of relief amount if last year's budget is expired.</summary>
    /// <remarks>Relief amount from last year is only valid until 1st of july.</remarks>
    [Fact]
    public async Task ProcessBudgetAsync_RefundAppointmentOfLastYear()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var companyId = Guid.NewGuid();
        var budget = new Budget
        {
            Customer = new()
        };
        List<Appointment> existingAppointment =
        [
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 50,
                Id = Guid.NewGuid()
            },
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today.AddYears(-1),
                Costs = 30,
                CostsLastYearBudget = 20,
                Id = Guid.NewGuid()
            }
        ];
        _budgetRepository.GetCurrentBudgetAsync(companyId, Arg.Any<Guid>())
            .Returns(budget);
        _appointmentRepository.GetAppointmentsAsync(companyId, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), null, Arg.Any<Guid>())
            .Returns(existingAppointment.ToArray().ToList());

        // Act
        await _sut.ProcessBudgetAsync(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(20);
        existingAppointment[0].CostsLastYearBudget.Should().Be(30);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    /// <summary>Tests the refunding of relief amount.</summary>
    /// <remarks>Used budget from last year should be switched with the existing appointment</remarks>
    [Fact]
    public async Task ProcessBudgetAsync_RefundOfLastYearReliefAmount()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var companyId = Guid.NewGuid();
        var budget = new Budget
        {
            Customer = new()
        };
        List<Appointment> existingAppointment =
        [
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 50,
                Id = Guid.NewGuid()
            },
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 30,
                CostsLastYearBudget = 20,
                Id = Guid.NewGuid()
            }
        ];
        _budgetRepository.GetCurrentBudgetAsync(companyId, Arg.Any<Guid>())
            .Returns(budget);
        _appointmentRepository.GetAppointmentsAsync(companyId, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), null, Arg.Any<Guid>())
            .Returns(existingAppointment.ToArray().ToList());

        // Act
        await _sut.ProcessBudgetAsync(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(30, "only 20 could be switched over");
        existingAppointment[0].CostsLastYearBudget.Should().Be(20, "the moved budget from the previous year");
        budget.ReliefAmount.Should().Be(30, "refunded from budget switched appointment");
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    /// <summary>Tests the refunding of relief amount to multiple appointments.</summary>
    /// <remarks>Used budget from last year should be switched with the existing appointments</remarks>
    [Fact]
    public async Task ProcessBudgetAsync_RefundOfLastYearReliefAmountToMultipleAppointments()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var companyId = Guid.NewGuid();
        var budget = new Budget
        {
            Customer = new()
        };
        List<Appointment> existingAppointment =
        [
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 10,
                Id = Guid.NewGuid()
            },
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 10,
                Id = Guid.NewGuid()
            },
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 0,
                CostsLastYearBudget = 50,
                Id = Guid.NewGuid()
            }
        ];
        _budgetRepository.GetCurrentBudgetAsync(companyId, Arg.Any<Guid>())
            .Returns(budget);
        _appointmentRepository.GetAppointmentsAsync(companyId, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), null, Arg.Any<Guid>())
            .Returns(existingAppointment.ToArray().ToList());

        // Act
        await _sut.ProcessBudgetAsync(existingAppointment[2]);

        // Assert
        existingAppointment[0].Costs.Should().Be(0);
        existingAppointment[0].CostsLastYearBudget.Should().Be(10);
        existingAppointment[1].Costs.Should().Be(0);
        existingAppointment[1].CostsLastYearBudget.Should().Be(10);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(30);
    }

    /// <summary>Tests the refunding of relief amount.</summary>
    /// <remarks>Relief amount from longer ago than two years must not be refunded.</remarks>
    [Fact]
    public async Task ProcessBudgetAsync_RefundOfTwoYearsAgoReliefAmount()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var companyId = Guid.NewGuid();
        var budget = new Budget
        {
            Customer = new()
        };
        List<Appointment> existingAppointment =
        [
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today,
                Costs = 50,
                Id = Guid.NewGuid()
            },
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = _dateTimeService.Today.AddYears(-2),
                Costs = 30,
                CostsLastYearBudget = 20,
                Id = Guid.NewGuid()
            }
        ];
        _budgetRepository.GetCurrentBudgetAsync(companyId, Arg.Any<Guid>())
            .Returns(budget);
        _appointmentRepository.GetAppointmentsAsync(companyId, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), null, Arg.Any<Guid>())
            .Returns(existingAppointment.ToArray().ToList());

        // Act
        await _sut.ProcessBudgetAsync(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(50);
        existingAppointment[0].CostsLastYearBudget.Should().Be(0);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    // test refunding if last year budget is expired
}
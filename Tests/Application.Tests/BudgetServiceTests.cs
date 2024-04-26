namespace Application.Tests;

using AutoFixture;
using Curacaru.Backend.Application.Services;
using Curacaru.Backend.Application.Services.Implementations;
using Curacaru.Backend.Core.Entities;
using Curacaru.Backend.Core.Enums;
using Curacaru.Backend.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;

public class BudgetServiceTests
{
    private readonly IAppointmentRepository _appointmentRepository = Substitute.For<IAppointmentRepository>();

    private readonly IBudgetRepository _budgetRepository = Substitute.For<IBudgetRepository>();

    private readonly ICompanyRepository _companyRepository = Substitute.For<ICompanyRepository>();

    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();

    private readonly IFixture _fixture = new Fixture();

    private readonly BudgetService _sut;

    public BudgetServiceTests()
    {
        _sut = new(_appointmentRepository, _budgetRepository, _companyRepository, _dateTimeService);
        _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
    }

    /// <summary>Tests the refunding of relief amount if last year's budget is expired.</summary>
    /// <remarks>Relief amount from last year is only valid until 1st of july.</remarks>
    [Fact]
    public async Task RefundBudget_RefundAppointmentOfLastYear()
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
        await _sut.RefundBudget(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(20);
        existingAppointment[0].CostsLastYearBudget.Should().Be(30);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    /// <summary>Tests the refunding of care benefit.</summary>
    [Fact]
    public async Task RefundBudget_RefundCareBenefit()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var budget = new Budget
        {
            Customer = new()
        };
        var appointment = _fixture.Create<Appointment>();
        appointment.ClearanceType = ClearanceType.CareBenefit;
        appointment.Date = _dateTimeService.Today;

        _budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, Arg.Any<Guid>())
            .Returns(budget);

        // Act
        await _sut.RefundBudget(appointment);

        // Assert
        budget.CareBenefitAmount.Should().Be(appointment.Costs);
        budget.PreventiveCareAmount.Should().Be(0);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
        budget.SelfPayAmount.Should().Be(0);
    }

    /// <summary>Tests the refunding of relief amount.</summary>
    /// <remarks>Used budget from last year should be switched with the existing appointment</remarks>
    [Fact]
    public async Task RefundBudget_RefundOfExpiredLastYearReliefAmount()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 9, 13));
        _dateTimeService.Today.Returns(new DateOnly(2024, 9, 13));
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
        await _sut.RefundBudget(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(50);
        existingAppointment[0].CostsLastYearBudget.Should().Be(0);
        budget.ReliefAmount.Should().Be(30);
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    /// <summary>Tests the refunding of relief amount.</summary>
    /// <remarks>Used budget from last year should be switched with the existing appointment</remarks>
    [Fact]
    public async Task RefundBudget_RefundOfLastYearReliefAmount()
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
        await _sut.RefundBudget(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(30, "only 20 could be switched over");
        existingAppointment[0].CostsLastYearBudget.Should().Be(20, "the moved budget from the previous year");
        budget.ReliefAmount.Should().Be(30, "refunded from budget switched appointment");
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    /// <summary>Tests the refunding of relief amount to multiple appointments.</summary>
    /// <remarks>Used budget from last year should be switched with the existing appointments</remarks>
    [Fact]
    public async Task RefundBudget_RefundOfLastYearReliefAmountToMultipleAppointments()
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
        await _sut.RefundBudget(existingAppointment[2]);

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
    public async Task RefundBudget_RefundOfTwoYearsAgoReliefAmount()
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
        await _sut.RefundBudget(existingAppointment[1]);

        // Assert
        existingAppointment[0].Costs.Should().Be(50);
        existingAppointment[0].CostsLastYearBudget.Should().Be(0);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
    }

    /// <summary>Tests the refunding of preventive care.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RefundBudget_RefundPreventiveCare(bool isLastYear)
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var budget = new Budget
        {
            Customer = new()
        };
        var appointment = _fixture.Create<Appointment>();
        appointment.ClearanceType = ClearanceType.PreventiveCare;
        appointment.Date = isLastYear ? _dateTimeService.Today.AddYears(-1) : _dateTimeService.Today;

        _budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, Arg.Any<Guid>())
            .Returns(budget);

        // Act
        await _sut.RefundBudget(appointment);

        // Assert
        budget.CareBenefitAmount.Should().Be(0);
        budget.PreventiveCareAmount.Should().Be(isLastYear ? 0 : appointment.Costs);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
        budget.SelfPayAmount.Should().Be(0);
    }

    /// <summary>Tests the refunding of self payment.</summary>
    [Fact]
    public async Task RefundBudget_RefundSelfPayment()
    {
        // Arrange
        _dateTimeService.Now.Returns(new DateTime(2024, 2, 14));
        _dateTimeService.Today.Returns(new DateOnly(2024, 2, 14));
        var budget = new Budget
        {
            Customer = new()
        };
        var appointment = _fixture.Create<Appointment>();
        appointment.ClearanceType = ClearanceType.SelfPayment;
        appointment.Date = _dateTimeService.Today;

        _budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, Arg.Any<Guid>())
            .Returns(budget);

        // Act
        await _sut.RefundBudget(appointment);

        // Assert
        budget.CareBenefitAmount.Should().Be(0);
        budget.PreventiveCareAmount.Should().Be(0);
        budget.ReliefAmount.Should().Be(0);
        budget.ReliefAmountLastYear.Should().Be(0);
        budget.SelfPayAmount.Should().Be(appointment.Costs);
    }
}
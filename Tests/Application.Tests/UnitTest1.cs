namespace Application.Tests;

using AutoMapper;
using Curacaru.Backend.Application;
using Curacaru.Backend.Application.CQRS.Appointments;
using Curacaru.Backend.Application.Services;
using Curacaru.Backend.Core.Entities;
using Curacaru.Backend.Core.Enums;
using Curacaru.Backend.Infrastructure.repositories;
using Curacaru.Backend.Infrastructure.Repositories;
using Curacaru.Backend.Infrastructure.Services;
using FluentAssertions;
using NSubstitute;

public class UnitTest1
{
    private readonly IAppointmentRepository _appointmentRepository = Substitute.For<IAppointmentRepository>();

    private readonly IBudgetRepository _budgetRepository = Substitute.For<IBudgetRepository>();

    private readonly IDatabaseService _databaseService = Substitute.For<IDatabaseService>();

    private readonly IDateTimeService _dateTimeService = Substitute.For<IDateTimeService>();

    private readonly IEmployeeRepository _employeeRepository = Substitute.For<IEmployeeRepository>();

    private readonly IMapper _mapper;

    private readonly DeleteAppointmentRequestHandler _sut;

    public UnitTest1()
    {
        _mapper = new Mapper(new MapperConfiguration(o => o.AddProfile(new MappingProfile())));
        _sut = new(_appointmentRepository, _budgetRepository, _databaseService, _dateTimeService, _employeeRepository);
    }

    [Fact]
    public async Task ProcessBudgetAsync_TestRefundOfReliefAmount()
    {
        // Arrange
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
                Date = DateOnly.FromDateTime(DateTime.Today),
                Costs = 50,
                Id = Guid.NewGuid()
            },
            new()
            {
                ClearanceType = ClearanceType.ReliefAmount,
                CompanyId = companyId,
                Date = DateOnly.FromDateTime(DateTime.Today),
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
        existingAppointment[0].Costs.Should().Be(30);
        existingAppointment[0].CostsLastYearBudget.Should().Be(20);
        budget.ReliefAmount.Should().Be(30);
    }

    // Test if deleted from two years agp

    // Test if deleted and nothing to refund

    // Test refunding to multiple appointments

    // test refunding if last year budget is expired
}
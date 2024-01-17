namespace Curacaru.Backend.Application.CQRS.Deployment;

using Core.DTO.Deployment;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get deployments for an employee for a month.</summary>
public class DeploymentsRequest(
    Guid companyId,
    string authId,
    int year,
    int month) : IRequest<List<GetDeploymentDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class DeploymentsRequestHandler(IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository)
    : IRequestHandler<DeploymentsRequest, List<GetDeploymentDto>>
{
    public async Task<List<GetDeploymentDto>> Handle(DeploymentsRequest request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId)
                       ?? throw new BadRequestException("Mitarbeiter existiert nicht.");

        var startDate = new DateOnly(request.Year, request.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, startDate, endDate, employee.Id, null);

        var deployments = appointments
            .Where(o => o.Customer.InsuranceStatus != null)
            .GroupBy(o => new Tuple<Guid, InsuranceStatus?>(o.CustomerId, o.Customer.InsuranceStatus))
            .Select(
                group => new GetDeploymentDto
                {
                    CustomerId = group.Key.Item1,
                    CustomerName = group.First().Customer.FirstName + " " + group.First().Customer.LastName,
                    InsuranceStatus = group.Key.Item2!.Value
                })
            .ToList();

        return deployments;
    }
}
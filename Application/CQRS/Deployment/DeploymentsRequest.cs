namespace Curacaru.Backend.Application.CQRS.Deployment;

using AutoMapper;
using Core.DTO.Deployment;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get deployments for an employee for a month.</summary>
public class DeploymentsRequest(
    Guid companyId,
    string authId) : IRequest<List<GetDeploymentDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class DeploymentsRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<DeploymentsRequest, List<GetDeploymentDto>>
{
    public async Task<List<GetDeploymentDto>> Handle(DeploymentsRequest request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId)
                       ?? throw new BadRequestException("Mitarbeiter existiert nicht.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var customers = await customerRepository.GetCustomersAsync(request.CompanyId, user!.IsManager ? null : user.Id);

        //var deployments = appointments
        //    .Where(o => o.Customer.InsuranceStatus != null)
        //    .GroupBy(o => new Tuple<Guid, InsuranceStatus?>(o.CustomerId, o.Customer.InsuranceStatus))
        //    .Select(
        //        group => new GetDeploymentDto
        //        {
        //            CustomerId = group.Key.Item1,
        //            CustomerName = group.First().Customer.FirstName + " " + group.First().Customer.LastName,
        //            InsuranceStatus = group.Key.Item2!.Value
        //        })
        //    .ToList();

        return mapper.Map<List<GetDeploymentDto>>(customers);
    }
}
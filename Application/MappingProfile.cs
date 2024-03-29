﻿namespace Curacaru.Backend.Application;

using AutoMapper;
using Core.DTO;
using Core.DTO.Appointment;
using Core.DTO.Budget;
using Core.DTO.Company;
using Core.DTO.Customer;
using Core.DTO.Insurance;
using Core.DTO.TimeTracker;
using Core.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateAppointmentMappings();
        CreateCustomerMappings();

        CreateMap<Budget, GetBudgetListEntryDto>()
            .ForMember(
                o => o.TotalAmount,
                src => src.MapFrom(o => o.CareBenefitAmount + o.PreventiveCareAmount + o.ReliefAmount + o.ReliefAmountLastYear + o.SelfPayAmount))
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.Customer.FirstName} {o.Customer.LastName}".Trim()));

        CreateMap<Company, GetCompanyDto>();
        CreateMap<Company, GetCompanyPricesDto>();

        CreateMap<Insurance, GetInsuranceDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""));

        CreateMap<Employee, GetEmployeeBase>()
            .ForMember(o => o.Name, src => src.MapFrom(o => (o.FirstName + " " + o.LastName).Trim()));

        CreateMap<WorkingTimeReport, GetWorkingTimeReportDto>();
    }

    private void CreateAppointmentMappings()
    {
        CreateMap<Appointment, GetAppointmentListEntryDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.Customer.ZipCity != null ? o.Customer.ZipCity.City : ""))
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.Customer.FirstName} {o.Customer.LastName}".Trim()))
            .ForMember(o => o.EmployeeName, src => src.MapFrom(o => $"{o.Employee.FirstName} {o.Employee.LastName}".Trim()))
            .ForMember(o => o.Phone, src => src.MapFrom(o => o.Customer.Phone))
            .ForMember(o => o.Street, src => src.MapFrom(o => o.Customer.Street))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.Customer.ZipCode))
            .ForMember(
                o => o.EmployeeReplacementName,
                src => src.MapFrom(o => o.EmployeeReplacement != null ? (o.EmployeeReplacement.FirstName + " " + o.EmployeeReplacement.LastName).Trim() : ""));

        CreateMap<Appointment, GetAppointmentDto>()
            .ForMember(o => o.IsSignedByCustomer, src => src.MapFrom(o => !string.IsNullOrEmpty(o.SignatureCustomer)))
            .ForMember(o => o.IsSignedByEmployee, src => src.MapFrom(o => !string.IsNullOrEmpty(o.SignatureEmployee)));

        CreateMap<AddAppointmentDto, Appointment>();
        CreateMap<Appointment, Appointment>();
    }

    private void CreateCustomerMappings()
    {
        CreateMap<Customer, GetCustomerListEntryDto>()
            .ForMember(o => o.AssociatedEmployeeName, src => src.MapFrom(o => o.AssociatedEmployee != null ? o.AssociatedEmployee.FullName : ""))
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.ZipCode : ""));

        CreateMap<Customer, GetCustomerDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.ZipCode : ""));

        CreateMap<Customer, GetMinimalCustomerListEntryDto>()
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.FirstName} {o.LastName}"))
            .ForMember(o => o.CustomerId, src => src.MapFrom(o => o.Id));

        CreateMap<Customer, GetCustomerBudgetDto>()
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.FirstName} {o.LastName}"))
            .ForMember(o => o.CustomerId, src => src.MapFrom(o => o.Id));

        CreateMap<AddCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();
    }
}
﻿namespace Curacaru.Backend.Application;

using AutoMapper;
using Core.DTO;
using Core.DTO.Appointment;
using Core.DTO.Budget;
using Core.DTO.Company;
using Core.DTO.ContactForm;
using Core.DTO.Customer;
using Core.DTO.Insurance;
using Core.DTO.Payment;
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
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.Customer.LastName},  {o.Customer.FirstName}".Trim()));

        CreateMap<Company, GetCompanyDto>();
        CreateMap<Company, GetCompanyPricesDto>();

        CreateMap<Insurance, GetInsuranceDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""));

        CreateMap<Employee, GetEmployeeBase>()
            .ForMember(o => o.Name, src => src.MapFrom(o => (o.FirstName + " " + o.LastName).Trim()));

        CreateMap<WorkingTimeReport, GetWorkingTimeReportDto>();

        CreateMap<Subscription, GetSubscriptionDto>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.IsCanceled && (src.PeriodEnd == null || src.PeriodEnd >= DateTime.Today)));
    }

    private void CreateAppointmentMappings()
    {
        CreateMap<Appointment, GetAppointmentListEntryDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.Customer.ZipCity != null ? o.Customer.ZipCity.City : ""))
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.Customer.LastName}, {o.Customer.FirstName}".Trim()))
            .ForMember(o => o.EmployeeName, src => src.MapFrom(o => $"{o.Employee.FirstName} {o.Employee.LastName}".Trim()))
            .ForMember(o => o.Phone, src => src.MapFrom(o => o.Customer.Phone))
            .ForMember(o => o.Street, src => src.MapFrom(o => o.Customer.Street))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.Customer.ZipCode))
            .ForMember(o => o.IsBirthday, src => src.MapFrom(o => o.Customer.BirthDate.Day == o.Date.Day && o.Customer.BirthDate.Month == o.Date.Month))
            .ForMember(o => o.IsSignedByCustomer, src => src.MapFrom(o => !string.IsNullOrEmpty(o.SignatureCustomer)))
            .ForMember(o => o.IsSignedByEmployee, src => src.MapFrom(o => !string.IsNullOrEmpty(o.SignatureEmployee)))
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
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ZipCity != null ? src.ZipCity.City : ""))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(o => o.Id).ToList()))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCity != null ? src.ZipCity.ZipCode : ""));

        CreateMap<Customer, GetMinimalCustomerListEntryDto>()
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.LastName}, {o.FirstName}"))
            .ForMember(o => o.CustomerId, src => src.MapFrom(o => o.Id));

        CreateMap<Customer, GetCustomerBudgetDto>()
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => $"{o.LastName}, {o.FirstName}"))
            .ForMember(o => o.CustomerId, src => src.MapFrom(o => o.Id));

        CreateMap<AddCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        CreateMap<AddContactInfoDto, Customer>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.EmergencyContactName, src => src.MapFrom(o => o.Contact ?? ""))
            .ForMember(dest => dest.EmergencyContactPhone, src => src.MapFrom(o => o.Contact == null ? "" : o.Phone))
            .ForMember(dest => dest.Phone, src => src.MapFrom(o => o.Contact == null ? o.Phone : ""));
    }
}
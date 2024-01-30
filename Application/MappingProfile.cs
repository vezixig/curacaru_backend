﻿namespace Curacaru.Backend.Application;

using AutoMapper;
using Core.DTO;
using Core.DTO.Appointment;
using Core.DTO.Company;
using Core.DTO.Customer;
using Core.DTO.Insurance;
using Core.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateAppointmentMappings();
        CreateCustomerMappings();

        CreateMap<Company, GetCompanyDto>();

        CreateMap<Insurance, GetInsuranceDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""));

        CreateMap<Employee, GetEmployeeBase>()
            .ForMember(o => o.Name, src => src.MapFrom(o => (o.FirstName + " " + o.LastName).Trim()));
    }

    private void CreateAppointmentMappings()
    {
        CreateMap<Appointment, GetAppointmentListEntryDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.Customer.ZipCity != null ? o.Customer.ZipCity.City : ""))
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => (o.Customer.FirstName + " " + o.Customer.LastName).Trim()))
            .ForMember(o => o.EmployeeName, src => src.MapFrom(o => (o.Employee.FirstName + " " + o.Employee.LastName).Trim()))
            .ForMember(o => o.Latitude, src => src.MapFrom(o => o.Customer.Latitude))
            .ForMember(o => o.Longitude, src => src.MapFrom(o => o.Customer.Longitude))
            .ForMember(o => o.Phone, src => src.MapFrom(o => o.Customer.Phone))
            .ForMember(o => o.Street, src => src.MapFrom(o => o.Customer.Street))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.Customer.ZipCode))
            .ForMember(
                o => o.EmployeeReplacementName,
                src => src.MapFrom(o => o.EmployeeReplacement != null ? (o.EmployeeReplacement.FirstName + " " + o.EmployeeReplacement.LastName).Trim() : ""));

        CreateMap<Appointment, GetAppointmentDto>();

        CreateMap<AddAppointmentDto, Appointment>();
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

        CreateMap<AddCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();
    }
}
namespace Curacaru.Backend.Application;

using AutoMapper;
using Core.DTO;
using Core.DTO.Company;
using Core.DTO.Insurance;
using Core.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateAppointmentMappings();
        CreateCustomerMappings();

        CreateMap<Company, GetCompanyDto>();

        CreateMap<Insurance, GetInsuranceDto>();

        CreateMap<Employee, GetEmployeeBase>()
            .ForMember(o => o.Name, src => src.MapFrom(o => (o.FirstName + " " + o.LastName).Trim()));
    }

    private void CreateAppointmentMappings()
    {
        CreateMap<Appointment, GetAppointmentListEntryDto>()
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => (o.Customer.FirstName + " " + o.Customer.LastName).Trim()))
            .ForMember(o => o.City, src => src.MapFrom(o => o.Customer.ZipCity != null ? o.Customer.ZipCity.City : ""))
            .ForMember(o => o.EmployeeName, src => src.MapFrom(o => (o.Employee.FirstName + " " + o.Employee.LastName).Trim()))
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

        CreateMap<AddCustomerDto, Customer>();
    }
}
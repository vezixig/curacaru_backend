namespace Curacaru.Backend.Application;

using AutoMapper;
using Core.DTO;
using Core.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, GetAppointmentListEntryDto>()
            .ForMember(o => o.CustomerName, src => src.MapFrom(o => (o.Customer.FirstName + " " + o.Customer.LastName).Trim()))
            .ForMember(o => o.City, src => src.MapFrom(o => o.Customer.ZipCity != null ? o.Customer.ZipCity.City : ""))
            .ForMember(o => o.EmployeeName, src => src.MapFrom(o => (o.Employee.FirstName + " " + o.Employee.LastName).Trim()))
            .ForMember(
                o => o.EmployeeReplacementName,
                src => src.MapFrom(o => o.EmployeeReplacement != null ? (o.EmployeeReplacement.FirstName + " " + o.EmployeeReplacement.LastName).Trim() : ""));

        CreateMap<Customer, GetCustomerListEntryDto>()
            .ForMember(o => o.AssociatedEmployeeName, src => src.MapFrom(o => o.AssociatedEmployee != null ? o.AssociatedEmployee.FullName : ""))
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.ZipCode : ""));

        CreateMap<Customer, GetCustomerDto>()
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.ZipCode : ""));

        CreateMap<AddCustomerDto, Customer>();

        CreateMap<Insurance, GetInsuranceDto>();
    }
}
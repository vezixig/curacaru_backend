namespace Curacaru.Backend.Application;

using AutoMapper;
using Core.DTO;
using Core.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, GetCustomerListEntryDto>()
            .ForMember(o => o.AssociatedEmployeeName, src => src.MapFrom(o => o.AssociatedEmployee != null ? o.AssociatedEmployee.FullName : ""))
            .ForMember(o => o.City, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.City : ""))
            .ForMember(o => o.ZipCode, src => src.MapFrom(o => o.ZipCity != null ? o.ZipCity.ZipCode : ""));
        CreateMap<AddCustomerDto, Customer>();
    }
}
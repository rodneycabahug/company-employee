using AutoMapper;
using company_employees_entities.Models;
using company_employees_shared.DataTransferObjects;

namespace company_employees_webapi.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(e => $"{e.Address}, {e.Country}"));

        CreateMap<Employee, EmployeeDto>();

        CreateMap<CompanyForCreationDto, Company>();

        CreateMap<CompanyForUpdateDto, Company>();

        CreateMap<EmployeeForCreationDto, Employee>();

        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
    }
}

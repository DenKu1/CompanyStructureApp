using AutoMapper;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.DTOs;

namespace CompanyStructureApp.CLI.Mapper
{
    class MapperProfile : Profile
    {
        public MapperProfile()
        {         
            CreateMap<EmployeeDTO, Employee>().ReverseMap();
        }
    }
}

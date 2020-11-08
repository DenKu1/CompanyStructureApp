using AutoMapper;
using CompanyStructureApp.Domain.Core.Concrete;
using CompanyStructureApp.DTOs;
using CompanyStructureApp.WEB.Models;

namespace CompanyStructureApp.WEB.Mapper
{
    class MapperProfile : Profile
    {
        public MapperProfile()
        {         
            CreateMap<EmployeeDTO, Employee>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeVM>().ForMember(e => e.SuperiorId, opt => opt.Ignore()).ReverseMap();
        }
    }
}

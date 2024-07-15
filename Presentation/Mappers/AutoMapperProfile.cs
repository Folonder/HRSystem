using AutoMapper;
using Domain.Entities;
using Presentation.Models;
using Presentation.Models.Department;
using Presentation.Models.Employee;

namespace Presentation.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DepartmentCreateModel, Department>();
        CreateMap<Department, DepartmentCreateModel>();
        CreateMap<DepartmentEditModel, Department>();
        CreateMap<Department, DepartmentEditModel>();
        CreateMap<DepartmentCreateModel, DepartmentHistory>()
            .ForMember(dest => dest.DepartmentId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OpenDate,
                opt => opt.MapFrom(src => src.OpenDate));
        
        CreateMap<Employee, EmployeeCreateModel>();
        CreateMap<EmployeeCreateModel, Employee>();
        CreateMap<Employee, EmployeeEditModel>();
        CreateMap<EmployeeEditModel, Employee>();
        
        CreateMap<Department, DepartmentTreeViewModel>().ForMember(dest => dest.SubDepartments, dest => dest.Ignore());
    }
}
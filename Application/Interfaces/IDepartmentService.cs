using Domain.Entities;

namespace Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<IEnumerable<Department>> GetAllDepartmentsExceptGivenAsync(int departmentId);
    Task<Department> GetDepartmentByIdAsync(int id);
    Task<Department> GetDepartmentByIdIncludeParentAsync(int departmentId);
    Task AddDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(int departmentId);
}
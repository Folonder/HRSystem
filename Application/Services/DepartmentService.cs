using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await _departmentRepository.GetAllAsync();
    }
    
    public async Task<IEnumerable<Department>> GetAllDepartmentsExceptGivenAsync(int departmentId)
    {
        return await _departmentRepository.GetWhereAsync(d => d.Id != departmentId);
    }

    public async Task<Department> GetDepartmentByIdAsync(int id)
    {
        return await _departmentRepository.GetByIdAsync(id);
    }

    public async Task<Department> GetDepartmentByIdIncludeParentAsync(int departmentId)
    {
        return (await _departmentRepository.GetWhereWithIncludeAsync(d => d.Id == departmentId, d => d.ParentDepartment)).First();
    }

    public async Task AddDepartmentAsync(Department department)
    {
        await _departmentRepository.AddAsync(department);
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        await _departmentRepository.UpdateAsync(department);
    }

    public async Task DeleteDepartmentAsync(int departmentId)
    {
        await _departmentRepository.DeleteAsync(departmentId);
    }
}

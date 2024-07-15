using Domain.Entities;

namespace Application.Interfaces;

public interface IDepartmentHistoryService
{
    Task<IEnumerable<DepartmentHistory>> GetAllDepartmentHistoriesAsync();
    Task<DepartmentHistory> GetDepartmentHistoryByIdAsync(int departmentHistoryId);
    Task<DepartmentHistory> GetOpenedDepartmentHistoryByDepartmentId(int departmentId);
    Task AddDepartmentHistoryAsync(DepartmentHistory departmentHistory);
    Task UpdateDepartmentHistoryAsync(DepartmentHistory departmentHistory);
    Task DeleteDepartmentHistoryAsync(int departmentHistoryId);
    Task CloseDepartmentAndSubDepartmentsAsync(int departmentId, DateTime closeDate);
    Task<IEnumerable<Department>> GetAllOpenedDepartmentsAsync(DateTime date);
    Task<IEnumerable<Department>> GetAllOpenedRootDepartmentsAsync(DateTime date);
}
using Domain.Entities;

namespace Application.Interfaces;

public interface IEmployeesHistoryService
{
    Task<IEnumerable<EmployeeHistory>> GetAllEmployeesHistories();
    Task<EmployeeHistory> GetEmployeesHistoryById(int employeeHistoryId);
    Task AddEmployeeHistoryAsync(EmployeeHistory employeeHistory);
    Task UpdateEmployeeHistoryAsync(EmployeeHistory employeeHistory);
    Task DeleteEmployeeHistoryAsync(int employeeHistoryId);
    Task<IEnumerable<Employee>> GetAllEmployeesNotInDepartmentAsync(int departmentId);
    Task<IEnumerable<Employee>> GetAllEmployeesInDepartmentAsync(int departmentId);
    Task FireEmployeesInDepartment(DepartmentHistory departmentHistory);
    Task FireEmployee(int employeeId, int departmentId, DateTime fireDate);
    Task<IEnumerable<EmployeeHistory>> GetAllEmployeesInDepartmentByPeriod(int departmentId, DateTime startDate,
        DateTime endDate);
}
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class EmployeesHistoryService : IEmployeesHistoryService
{
    private readonly IEmployeesHistoryRepository _employeesHistoryRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesHistoryService(IEmployeesHistoryRepository employeesHistoryRepository, IEmployeeRepository employeeRepository)
    {
        _employeesHistoryRepository = employeesHistoryRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeHistory>> GetAllEmployeesHistories()
    {
        return await _employeesHistoryRepository.GetAllAsync();
    }

    public async Task<EmployeeHistory> GetEmployeesHistoryById(int employeeHistoryId)
    {
        return await _employeesHistoryRepository.GetByIdAsync(employeeHistoryId);
    }
    
    public async Task AddEmployeeHistoryAsync(EmployeeHistory employeeHistory)
    {
        await _employeesHistoryRepository.AddAsync(employeeHistory);
    }

    public async Task UpdateEmployeeHistoryAsync(EmployeeHistory employeeHistory)
    {
        await _employeesHistoryRepository.UpdateAsync(employeeHistory);
    }

    public async Task DeleteEmployeeHistoryAsync(int employeeHistoryId)
    {
        await _employeesHistoryRepository.DeleteAsync(employeeHistoryId);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesNotInDepartmentAsync(int departmentId)
    {
        var employeeHistories = await _employeesHistoryRepository
            .GetWhereAsync(eh => eh.DepartmentId == departmentId &&
                                 eh.FireDate == null);
        var employeeIdsInDepartment = employeeHistories.Select(eh => eh.EmployeeId).ToList();
        var employees = await _employeeRepository.GetWhereAsync(e => !employeeIdsInDepartment.Contains(e.Id));
        return employees;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesInDepartmentAsync(int departmentId)
    {
        return (await _employeesHistoryRepository.GetWhereWithIncludeAsync(
            eh => eh.DepartmentId == departmentId &&
                  eh.FireDate == null, eh => eh.Employee))
            .Select(eh => eh.Employee);
    }

    public async Task FireEmployeesInDepartment(DepartmentHistory departmentHistory)
    {
        var firedEmployeesHistories = await _employeesHistoryRepository
            .GetWhereAsync(eh => eh.DepartmentId == departmentHistory.DepartmentId &&
                                 eh.FireDate == null);
        foreach (var firedEmployeesHistory in firedEmployeesHistories)
        {
            firedEmployeesHistory.FireDate = departmentHistory.CloseDate;
            await _employeesHistoryRepository.UpdateAsync(firedEmployeesHistory);
        }
    }

    public async Task FireEmployee(int employeeId, int departmentId, DateTime fireDate)
    {
        var employeesHistory = (await _employeesHistoryRepository
            .GetWhereAsync(eh => eh.EmployeeId == employeeId &&
                                 eh.DepartmentId == departmentId &&
                                 eh.FireDate == null)).FirstOrDefault();
        employeesHistory!.FireDate = fireDate;
        await _employeesHistoryRepository.UpdateAsync(employeesHistory);
    }

    public async Task<IEnumerable<EmployeeHistory>> GetAllEmployeesInDepartmentByPeriod(int departmentId, DateTime startDate,
        DateTime endDate)
    {
        return await _employeesHistoryRepository.GetWhereWithIncludeAsync(
            eh => eh.DepartmentId == departmentId && eh.HireDate <= endDate &&
                  (eh.FireDate == null || eh.FireDate >= startDate), eh => eh.Employee);
    }
    
}
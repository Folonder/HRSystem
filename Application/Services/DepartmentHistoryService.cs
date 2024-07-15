using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class DepartmentHistoryService : IDepartmentHistoryService
{
    private readonly IDepartmentHistoryRepository _departmentHistoryRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeesHistoryService _employeesHistoryService;

    public DepartmentHistoryService(IDepartmentHistoryRepository departmentHistoryRepository,
        IDepartmentRepository departmentRepository, IEmployeesHistoryService employeesHistoryService)
    {
        _departmentHistoryRepository = departmentHistoryRepository;
        _departmentRepository = departmentRepository;
        _employeesHistoryService = employeesHistoryService;
    }

    public async Task<IEnumerable<DepartmentHistory>> GetAllDepartmentHistoriesAsync()
    {
        return await _departmentHistoryRepository.GetAllAsync();
    }

    public async Task<DepartmentHistory> GetDepartmentHistoryByIdAsync(int departmentHistoryId)
    {
        return await _departmentHistoryRepository.GetByIdAsync(departmentHistoryId);
    }

    public async Task<DepartmentHistory> GetOpenedDepartmentHistoryByDepartmentId(int departmentId)
    {
        return (await _departmentHistoryRepository.GetWhereAsync(dh =>
            dh.DepartmentId == departmentId && dh.CloseDate == null)).FirstOrDefault() ?? throw new NotFoundException("History of department is not found");
    }

    public async Task AddDepartmentHistoryAsync(DepartmentHistory departmentHistory)
    {
        await _departmentHistoryRepository.AddAsync(departmentHistory);
    }

    public async Task UpdateDepartmentHistoryAsync(DepartmentHistory departmentHistory)
    {
        await _departmentHistoryRepository.UpdateAsync(departmentHistory);
    }

    public async Task DeleteDepartmentHistoryAsync(int departmentHistoryId)
    {
        await _departmentHistoryRepository.DeleteAsync(departmentHistoryId);
    }

    public async Task CloseDepartmentAndSubDepartmentsAsync(int departmentId, DateTime closeDate)
    {
        var departmentHistory =
            (await _departmentHistoryRepository.GetWhereAsync(dh =>
                dh.DepartmentId == departmentId && dh.CloseDate == null)).FirstOrDefault();

        if (departmentHistory != null)
        {
            departmentHistory.CloseDate = closeDate;
            await _departmentHistoryRepository.UpdateAsync(departmentHistory);
            await _employeesHistoryService.FireEmployeesInDepartment(departmentHistory);
        }

        var subDepartments = (await _departmentRepository
            .GetWhereAsync(d => d.ParentDepartmentId == departmentId)).ToList();

        foreach (var subDept in subDepartments)
        {
            await CloseDepartmentAndSubDepartmentsAsync(subDept.Id, closeDate);
        }
    }
    
    public async Task<IEnumerable<Department>> GetAllOpenedDepartmentsAsync(DateTime date)
    {
        return (await _departmentHistoryRepository.GetWhereWithIncludeAsync(
            dh => dh.OpenDate <= date && (dh.CloseDate == null || dh.CloseDate >= date),
            dh => dh.Department.ParentDepartment)).Select(dh => dh.Department);
    }

    public async Task<IEnumerable<Department>> GetAllOpenedRootDepartmentsAsync(DateTime date)
    {
        var departmentHistories = await _departmentHistoryRepository.GetWhereWithIncludeAsync(
            dh => dh.OpenDate <= date && (dh.CloseDate == null || dh.CloseDate >= date),
            dh => dh.Department.SubDepartments);

        return departmentHistories
            .Where(dh => dh.Department.ParentDepartmentId == null)
            .Select(dh => dh.Department);
    }
}
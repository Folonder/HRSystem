using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models.Department;
using Presentation.Models.Employee;

namespace Presentation.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IDepartmentHistoryService _departmentHistoryService;
    private readonly IEmployeesHistoryService _employeesHistoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentService departmentService,
        IDepartmentHistoryService departmentHistoryService,
        IEmployeesHistoryService employeesHistoryService,
        IMapper mapper, ILogger<DepartmentController> logger)
    {
        _departmentService = departmentService;
        _departmentHistoryService = departmentHistoryService;
        _employeesHistoryService = employeesHistoryService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> Index(bool showOpenOnly = false)
    {
        ViewBag.ShowOpenOnly = showOpenOnly;
        if (showOpenOnly)
        {
            return View(await _departmentHistoryService.GetAllOpenedDepartmentsAsync(DateTime.Now));
        }
        return View(await _departmentService.GetAllDepartmentsAsync());
    }

    public async Task<IActionResult> Create()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentCreateModel departmentCreateModel)
    {
        if (!ModelState.IsValid)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            return View(departmentCreateModel);
        }
        _logger.LogInformation($"Creating department: {departmentCreateModel}");
        var department = _mapper.Map<Department>(departmentCreateModel);
        await _departmentService.AddDepartmentAsync(department);

        var departmentHistory = _mapper.Map<DepartmentHistory>(departmentCreateModel);
        departmentHistory.Department = department;
        await _departmentHistoryService.AddDepartmentHistoryAsync(departmentHistory);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var department = await _departmentService.GetDepartmentByIdAsync(id);
        var otherDepartments = await _departmentService.GetAllDepartmentsExceptGivenAsync(id);

        ViewBag.Departments = new SelectList(otherDepartments, "Id", "Name");
        return View(_mapper.Map<DepartmentEditModel>(department));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DepartmentEditModel departmentEditModel)
    {
        if (!ModelState.IsValid)
        {
            var otherDepartments = await _departmentService.GetAllDepartmentsExceptGivenAsync(departmentEditModel.Id);

            ViewBag.Departments = new SelectList(otherDepartments, "Id", "Name");
            return View(_mapper.Map<DepartmentEditModel>(departmentEditModel));
        }
        _logger.LogInformation($"Editing department: {departmentEditModel}");
        await _departmentService.UpdateDepartmentAsync(_mapper.Map<Department>(departmentEditModel));

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var department = await _departmentService.GetDepartmentByIdIncludeParentAsync(id);
        return View(department);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _departmentService.GetDepartmentByIdAsync(id);
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.LogInformation($"Deleting department with id: {id}");
        await _departmentService.DeleteDepartmentAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Close(int id)
    {
        return View(new DepartmentCloseModel { DepartmentId = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(DepartmentCloseModel departmentCloseModel)
    {
        _logger.LogInformation($"Closing department: {departmentCloseModel}");
        await _departmentHistoryService.CloseDepartmentAndSubDepartmentsAsync(departmentCloseModel.DepartmentId,
            departmentCloseModel.CloseDate);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> HireEmployee(int departmentId)
    {
        var employeesNotInDepartment = await _employeesHistoryService.GetAllEmployeesNotInDepartmentAsync(departmentId);
        ViewBag.Employees = new SelectList(employeesNotInDepartment, "Id", "Name");
        return View(new EmployeeHistory());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HireEmployee(EmployeeHistory employeeHistory)
    {
        _logger.LogInformation($"Hiring employee {employeeHistory}");
        await _employeesHistoryService.AddEmployeeHistoryAsync(employeeHistory);

        return RedirectToAction(nameof(Index), new { id = employeeHistory.DepartmentId });
    }

    public async Task<IActionResult> FireEmployee(int departmentId)
    {
        var employeesInDepartment = await _employeesHistoryService.GetAllEmployeesInDepartmentAsync(departmentId);
        ViewBag.Employees = new SelectList(employeesInDepartment, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FireEmployee(EmployeeFireModel employeeFireModel)
    {
        _logger.LogInformation($"Firing employee {employeeFireModel}");
        await _employeesHistoryService.FireEmployee(employeeFireModel.EmployeeId,
            employeeFireModel.DepartmentId, employeeFireModel.FireDate);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult SelectDateOfDepartment()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DepartmentTreeOnDate(DateTime date)
    {
        var rootDepartments = await _departmentHistoryService.GetAllOpenedRootDepartmentsAsync(date);

        var treeViewModel = new List<DepartmentTreeViewModel>();

        foreach (var root in rootDepartments)
        {
            var viewModel = new DepartmentTreeViewModel();
            PopulateTreeViewModel(viewModel, root, 0);
            treeViewModel.Add(viewModel);
        }

        return View(treeViewModel);
    }

    private void PopulateTreeViewModel(DepartmentTreeViewModel viewModel, Department department, int level)
    {
        _mapper.Map(department, viewModel);
        viewModel.Level = level;

        foreach (var subDept in department.SubDepartments)
        {
            var subViewModel = new DepartmentTreeViewModel();
            PopulateTreeViewModel(subViewModel, subDept, level + 1);
            viewModel.SubDepartments.Add(subViewModel);
        }
    }

    public IActionResult SelectPeriodOfDepartmentEmployees(int departmentId)
    {
        return View(new PeriodOfDepartmentEmployeesModel { DepartmentId = departmentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EmployeePeriod(PeriodOfDepartmentEmployeesModel model)
    {
        return RedirectToAction(nameof(EmployeePeriodResults), model);
    }

    public async Task<IActionResult> EmployeePeriodResults(
        PeriodOfDepartmentEmployeesModel periodOfDepartmentEmployeesModel)
    {
        var employees = await _employeesHistoryService.GetAllEmployeesInDepartmentByPeriod(
            periodOfDepartmentEmployeesModel.DepartmentId,
            periodOfDepartmentEmployeesModel.StartDate, periodOfDepartmentEmployeesModel.EndDate);

        return View(employees);
    }
}
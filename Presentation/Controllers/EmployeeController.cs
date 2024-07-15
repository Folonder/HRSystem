using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Models.Employee;

namespace Presentation.Controllers;


public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return View(employees);
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeCreateModel employeeCreateModel)
    {
        _logger.LogInformation($"Creating employee {employeeCreateModel}");
        if (!ModelState.IsValid)
        {
            return View(employeeCreateModel);
        }

        var employee = _mapper.Map<Employee>(employeeCreateModel);
        await _employeeService.AddEmployeeAsync(employee);
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return View(_mapper.Map<EmployeeEditModel>(employee));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeEditModel employeeEditModel)
    {
        if (!ModelState.IsValid)
        {
            return View(employeeEditModel);
        }
        _logger.LogInformation($"Editing employee {employeeEditModel}");
        var employee = _mapper.Map<Employee>(employeeEditModel);
        await _employeeService.UpdateEmployeeAsync(employee);
        
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.LogInformation($"Deleting employee with id: {id}");
        await _employeeService.DeleteEmployeeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
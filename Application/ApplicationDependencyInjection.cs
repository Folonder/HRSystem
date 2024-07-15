using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeesHistoryService, EmployeesHistoryService>();
        services.AddScoped<IDepartmentHistoryService, DepartmentHistoryService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        return services;
    }
}
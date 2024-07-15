using Domain.Interfaces;
using Infrastructure.Configs;
using Infrastructure.Data;
using Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseMySql(sp.GetRequiredService<IOptions<DatabaseConfig>>().Value.ConnectionString, new MySqlServerVersion("8.0.0")));
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentHistoryRepository, DepartmentHistoryRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeesHistoryRepository, EmployeesHistoryRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}
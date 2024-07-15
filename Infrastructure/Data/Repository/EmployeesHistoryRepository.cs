using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Repository;

public class EmployeesHistoryRepository : Repository<EmployeeHistory>, IEmployeesHistoryRepository
{
    public EmployeesHistoryRepository(ApplicationDbContext context, ILogger<EmployeeHistory> logger) : base(context, logger)
    {
    }
}
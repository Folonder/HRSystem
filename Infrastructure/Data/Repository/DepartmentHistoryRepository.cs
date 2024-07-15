using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Repository;

public class DepartmentHistoryRepository : Repository<DepartmentHistory>, IDepartmentHistoryRepository
{

    public DepartmentHistoryRepository(ApplicationDbContext context, ILogger<DepartmentHistory> logger) : base(context, logger)
    {
    }
}
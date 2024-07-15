using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Repository;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context, ILogger<Department> logger) : base(context, logger)
    {
    }

    public override async Task DeleteAsync(int id)
    {
        var department = await Context.Departments
            .Include(d => d.SubDepartments)
            .FirstOrDefaultAsync(d => d.Id == id)
                         ?? throw new NotFoundException("Not found department for deleting");
        
        var subDepartments = department.SubDepartments;
        foreach (var subDepartment in subDepartments)
        {
            subDepartment.ParentDepartmentId = null;
        }
        
        await Context.SaveChangesAsync();

        await base.DeleteAsync(id);
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("departments")]
public class Department
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;
    
    [Column("parent_department_id")]
    public int? ParentDepartmentId { get; set; }
    
    public Department? ParentDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; } = null!;
    
    public override string ToString()
    {
        return $"Department: Id - {Id}, Name - {Name}, ParentDepartmentId - {ParentDepartmentId}";
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;


[Table("employees_history")]
public class EmployeeHistory
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
    
    [Column("department_id")]
    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
    
    [Column("hire_date", TypeName="date")]
    public DateTime HireDate { get; set; }

    [Column("fire_date", TypeName="date")]
    public DateTime? FireDate { get; set; }
    
    public override string ToString()
    {
        return $"Employee History: Id - {Id} EmployeeId - {EmployeeId}, DepartmentId - {DepartmentId}, HireDate - {HireDate}, FireDate - {FireDate}";
    }
}
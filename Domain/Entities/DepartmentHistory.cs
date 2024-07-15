using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("departments_history")]
public class DepartmentHistory
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("department_id")]
    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
    
    [Column("open_date", TypeName="date")]
    public DateTime OpenDate { get; set; }
    
    [Column("close_date", TypeName="date")]
    public DateTime? CloseDate { get; set; }
    
    public override string ToString()
    {
        return $"Department History: Id - {Id}, DepartmentId - {DepartmentId}, OpenDate - {OpenDate}, CloseDate - {CloseDate}";
    }
}
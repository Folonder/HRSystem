namespace Presentation.Models.Employee;

public class EmployeeFireModel
{
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public DateTime FireDate { get; set; }
    
    public override string ToString()
    {
        return $"Employee: EmployeeId - {EmployeeId}, DepartmentId - {DepartmentId}, FireDate - {FireDate}";
    }
}
namespace Presentation.Models.Department;

public class DepartmentCloseModel
{
    public int DepartmentId { get; set; }
    public DateTime CloseDate { get; set; }
    
    public override string ToString()
    {
        return $"Department: Id - {DepartmentId}, CloseDate - {CloseDate}";
    }
}
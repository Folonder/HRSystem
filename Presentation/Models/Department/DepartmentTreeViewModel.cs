namespace Presentation.Models.Department;

public class DepartmentTreeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Level { get; set; }
    public List<DepartmentTreeViewModel> SubDepartments { get; set; } = new();
}
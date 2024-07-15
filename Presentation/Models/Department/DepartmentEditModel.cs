using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Department;

public class DepartmentEditModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
    public string Name { get; set; } = null!;
    public int? ParentDepartmentId { get; set; }

    public override string ToString()
    {
        return $"Department: Name - {Name}, ParentDepartmentId - {ParentDepartmentId}";
    }
}
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Employee;

public class EmployeeCreateModel
{
    [Required(ErrorMessage = "Поле 'ФИО' обязательно для заполнения")]
    public string Name { get; set; } = null!;
    
    public override string ToString()
    {
        return $"Employee: Name - {Name}, ";
    }
}

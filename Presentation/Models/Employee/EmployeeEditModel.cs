using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Employee;

public class EmployeeEditModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Поле 'ФИО' обязательно для заполнения")]
    public string Name { get; set; } = null!;
    
    public override string ToString()
    {
        return $"Employee: Id - {Id}, Name - {Name}, ";
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("employees")]
public class Employee
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    public override string ToString()
    {
        return $"Employee: Id - {Id} Name - {Name}";
    }
}
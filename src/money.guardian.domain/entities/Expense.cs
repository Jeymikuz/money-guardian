using System.ComponentModel.DataAnnotations.Schema;

namespace money.guardian.domain.entities;

public class Expense : BaseEntity
{
    public string Name { get; set; }
    public decimal Value { get; set; }
    public ExpenseGroup Group { get; set; }
    public Guid? GroupId { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
}
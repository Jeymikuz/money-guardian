using System.ComponentModel.DataAnnotations.Schema;

namespace money.guardian.domain.entities;

public class ExpenseGroup : BaseEntity
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public string UserId { get; set; }
}
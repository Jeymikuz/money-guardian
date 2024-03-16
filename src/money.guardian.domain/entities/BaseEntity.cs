namespace money.guardian.domain.entities;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using money.guardian.domain.entities;

namespace money.guardian.infrastructure.events;

public sealed class BaseEntityEvents
{
    public static void EntityCreated(object sender, EntityEntryEventArgs e)
    {
        if (e.Entry.State != EntityState.Added)
            return;

        if (e.Entry.Entity is BaseEntity entity)
        {
            entity.CreatedAt = DateTimeOffset.Now;
        }
    }
    
    public static void EntityModified(object sender, EntityEntryEventArgs e)
    {
        if (e.Entry.State != EntityState.Modified)
            return;

        if (e.Entry.Entity is BaseEntity entity)
        {
            entity.UpdatedAt = DateTimeOffset.Now;
        }
    }
}
using Backend.Models.Base;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Backend.Models.Extensions
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var item in eventData.Context.ChangeTracker.Entries())
            {
                if (item is { State: Microsoft.EntityFrameworkCore.EntityState.Deleted, Entity: BaseEntity deletedEntity })
                {
                    item.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    deletedEntity.IsDeleted = true;
                }
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

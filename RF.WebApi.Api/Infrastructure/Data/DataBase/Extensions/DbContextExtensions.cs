using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace RF.WebApi.Infrastructure.Data.DataBase
{
    public static class DbContextExtensions
    {
        public static void SyncCollection<TEntity, TDto>(
            this DbContext context,
            ICollection<TEntity> existingCollection,
            IEnumerable<TDto>? incomingCollection,
            Func<TEntity, TDto, bool> identityEquality,
            IMapper mapper)
            where TEntity : class
            where TDto : class
        {
            if (incomingCollection == null) return;

            var existingList = existingCollection.ToList();
            var incomingList = incomingCollection.ToList();

            // 1. Remove items that are no longer in the incoming list
            // We check if the existing item matches ANY incoming item's identity
            var toRemove = existingList
                .Where(e => !incomingList.Any(d => identityEquality(e, d)))
                .ToList();

            foreach (var item in toRemove)
            {
                existingCollection.Remove(item);
                context.Remove(item);
            }

            // 2. Add or Update items
            foreach (var dto in incomingList)
            {
                // Find the existing item ONLY if the incoming DTO has a valid ID > 0
                // This ensures that two new items with Id=0 are never merged
                var existing = existingList.FirstOrDefault(e => identityEquality(e, dto));

                if (existing != null)
                {
                    // Update: Sync properties from DTO to the tracked object instance
                    mapper.Map(dto, existing);
                }
                else
                {
                    // Add: Map DTO to a new entity
                    var newEntity = mapper.Map<TEntity>(dto);
                    
                    // Since the global SaveChangesAsync override was removed, 
                    // we must handle the Id=0 issue here to prevent Identity Insert errors.
                    var idProp = typeof(TEntity).GetProperty("Id");
                    if (idProp != null && idProp.CanWrite)
                    {
                        var val = idProp.GetValue(newEntity);
                        if (val is int intVal && intVal == 0)
                        {
                            // Set to null if nullable, or skip for non-nullable (EF handles 0 for non-nullable Identity automatically)
                            if (Nullable.GetUnderlyingType(idProp.PropertyType) != null || !idProp.PropertyType.IsValueType)
                            {
                                idProp.SetValue(newEntity, null);
                            }
                        }
                    }

                    existingCollection.Add(newEntity);
                }
            }
        }
    }
}

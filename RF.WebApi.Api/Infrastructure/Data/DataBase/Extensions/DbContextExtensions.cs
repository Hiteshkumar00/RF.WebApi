using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace RF.WebApi.Infrastructure.Data.DataBase.Extensions
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
                var existing = existingList.FirstOrDefault(e => identityEquality(e, dto));

                if (existing != null)
                {
                    // Update: Sync properties from DTO to the tracked object instance
                    mapper.Map(dto, existing);
                }
                else
                {
                    // Add: Map DTO to a new entity and add to collection
                    var newEntity = mapper.Map<TEntity>(dto);
                    existingCollection.Add(newEntity);
                }
            }
        }
    }
}

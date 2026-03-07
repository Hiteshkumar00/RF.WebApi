using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Application.DTOs.Entity;
using RF.WebApi.Api.Application.DTOs.RelatedEntity;
using RF.WebApi.Api.Domain.Common;
using RF.WebApi.Api.Domain.Exceptions;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Infrastructure.Data.DataBase;

namespace RF.WebApi.Api.Infrastructure.Services
{
    public class EntityService : IEntityService
    {
        private readonly RFDBContext _context;
        private readonly IMapper _mapper;

        public EntityService(RFDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ServiceResponse<int>> CreateEntity(CreateEntityDto dto)
        {
            return ServiceResponse<int>.Execute(async err =>
            {
                var newEntity = _mapper.Map<Entity>(dto);
                
                // EF core navigation properties will automatically insert related entities
                _context.Entitys.Add(newEntity);
                await _context.SaveChangesAsync();

                return newEntity.Id ?? default;
            });
        }

        public Task<ServiceResponse<EntityDto>> GetEntityById(int id)
        {
            return ServiceResponse<EntityDto>.Execute(async err =>
            {
                var entity = await _context.Entitys
                    .Include(e => e.RelatedEntities)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (entity == null)
                {
                    err.AddError(EntityMessages.NotFound);
                    return default;
                }

                return _mapper.Map<EntityDto>(entity);
            });
        }

        public Task<ServiceResponse<EntityDto>> GetEntityByName(string entityName)
        {
            return ServiceResponse<EntityDto>.Execute(async err =>
            {
                var entity = await _context.Entitys
                    .Include(e => e.RelatedEntities)
                    .FirstOrDefaultAsync(e => e.EntityName == entityName);

                if (entity == null)
                {
                    err.AddError(EntityMessages.NotFound);
                    return default;
                }

                return _mapper.Map<EntityDto>(entity);
            });
        }

        public Task<ServiceResponse<bool>> UpdateEntity(UpdateEntityDto dto)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var entity = await _context.Entitys
                    .Include(e => e.RelatedEntities)
                    .FirstOrDefaultAsync(e => e.Id == dto.Id);

                if (entity == null)
                {
                    err.AddError(EntityMessages.NotFound);
                    return false;
                }
                
                // AutoMapper seamlessly handles syncing all scalar properties
                // And adding/updating/deleting the nested Collections (RelatedEntities)
                _mapper.Map(dto, entity);

                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<bool>> DeleteEntity(int id)
        {
            return ServiceResponse<bool>.Execute(async err =>
            {
                var entity = await _context.Entitys
                    .Include(e => e.RelatedEntities)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (entity == null)
                {
                    err.AddError(EntityMessages.NotFound);
                    return false;
                }

                if (entity.RelatedEntities.Any())
                {
                    _context.RelatedEntitys.RemoveRange(entity.RelatedEntities);
                }

                _context.Entitys.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            });
        }

        public Task<ServiceResponse<List<EntityDto>>> GetAllEntities()
        {
            return ServiceResponse<List<EntityDto>>.Execute(async err =>
            {
                var entities = await _context.Entitys
                    .Include(e => e.RelatedEntities)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<EntityDto>>(entities);
            });
        }
    }
}

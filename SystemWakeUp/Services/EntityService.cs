using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemWakeUp.DBHandler;
using SystemWakeUp.DBHandler.Entity;

namespace SystemWakeUp.Services
{
    public class EntityService
    {
        private readonly AppDbContext _context;

        public EntityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DBEntity>> GetAllEntitiesAsync()
        {
            return await _context.GetEntitiesAsync();
        }

        public async Task<DBEntity> GetEntityByIdAsync(int id)
        {
            return await _context.GetEntityByIdAsync(id);
        }

        public async Task AddEntityAsync(DBEntity entity)
        {
            await _context.AddEntityAsync(entity);
        }

        public async Task UpdateEntityAsync(DBEntity entity)
        {
            await _context.UpdateEntityAsync(entity);
        }

        public async Task DeleteEntityAsync(int id)
        {
            await _context.DeleteEntityAsync(id);
        }
    }
}

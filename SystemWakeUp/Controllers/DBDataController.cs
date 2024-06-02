using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemWakeUp.DBHandler;
using SystemWakeUp.DBHandler.Entity;
using SystemWakeUp.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemWakeUp.Controllers
{
    [ApiController]
    [Route("api/dbdata")]
    public class DBDataController : Controller
    {
        private readonly EntityService _entityService;

        public DBDataController(EntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DBEntity>>> GetEntities()
        {
            return await _entityService.GetAllEntitiesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DBEntity>> GetEntity(int id)
        {
            var entity = await _entityService.GetEntityByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult> AddEntity(DBEntity entity)
        {
            await _entityService.AddEntityAsync(entity);
            return CreatedAtAction(nameof(GetEntity), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEntity(int id, DBEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            await _entityService.UpdateEntityAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEntity(int id)
        {
            await _entityService.DeleteEntityAsync(id);
            return NoContent();
        }
    }
}


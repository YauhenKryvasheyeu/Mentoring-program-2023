using CatalogService.DataAccess;
using CatalogService.WebApi.Mappers;
using CatalogService.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly CatalogContext _context;
        public ItemsController(CatalogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllAsync([FromQuery] ItemParameters parameters)
        {
            var filteredItems = parameters.CategoryId > 0
                ? _context.Items.Where(item => item.CategoryId == parameters.CategoryId)
                : _context.Items;

            var items = await filteredItems
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(item => item.ToDto()).ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> AddAsync(ItemDto item)
        {
            if (_context.Categories.FirstOrDefault(category => category.Id == item.CategoryId) == null)
            {
                return BadRequest($"Category with id: {item.CategoryId} does not exist");
            }

            var result = _context.Items.Add(item.ToDataAccess());
            await _context.SaveChangesAsync();
            return Ok(result.Entity.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<ItemDto>> UpdateAsync(ItemDto itemDto)
        {
            var item = _context.Items.FirstOrDefault(c => c.Id == itemDto.Id);
            if (item == null)
            {
                return BadRequest($"Item with id: {itemDto.Id} does not exist");
            }

            if (_context.Categories.FirstOrDefault(category => category.Id == itemDto.CategoryId) == null)
            {
                return BadRequest($"Category with id: {itemDto.CategoryId} does not exist");
            }

            item.CategoryId = itemDto.CategoryId;
            item.Description = itemDto.Description;

            await _context.SaveChangesAsync();

            return Ok(item.ToDto());
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var item = _context.Items.FirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                return BadRequest($"Item with id: {id} does not exist");
            }

            _context.Remove(item);

            await _context.SaveChangesAsync();

            return Ok($"Item id: {id} has been removed.");
        }
    }
}

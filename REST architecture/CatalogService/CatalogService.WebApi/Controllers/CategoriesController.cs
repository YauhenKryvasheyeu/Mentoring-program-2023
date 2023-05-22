using CatalogService.DataAccess;
using CatalogService.WebApi.Mappers;
using CatalogService.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CatalogContext _context;

        public CategoriesController(CatalogContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _context.Categories.Select(category => category.ToDto()).ToListAsync();
            return  Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetByIdAsync([Required]int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Id == id);
            if (category == null)
            {
                return BadRequest($"Category with id: {id} does not exist");
            }

            return Ok(category.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddAsync(CategoryDto category)
        {
            var result = _context.Categories.Add(category.ToDataAccess());
            await _context.SaveChangesAsync();
            return Ok(result.Entity.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<CategoryDto>> UpdateAsync(CategoryDto categoryDto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryDto.Id);
            if (category == null)
            {
                return BadRequest($"Category with id: {categoryDto.Id} does not exist");
            }

            category.Name = categoryDto.Name;

            await _context.SaveChangesAsync();

            return Ok(category.ToDto());
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([Required] int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return BadRequest($"Category with id: {id} does not exist");
            }

            _context.Items.RemoveRange(_context.Items.Where(item => item.CategoryId == category.Id));
            _context.Remove(category);
            await _context.SaveChangesAsync();

            return Ok($"Category id: {id} has been removed with all related items");
        }
    }
}

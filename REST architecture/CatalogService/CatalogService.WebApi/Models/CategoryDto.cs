using System.ComponentModel.DataAnnotations;

namespace CatalogService.WebApi.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CatalogService.WebApi.Models
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

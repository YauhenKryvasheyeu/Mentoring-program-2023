using System.ComponentModel.DataAnnotations;

namespace CatalogService.DataAccess.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }

        ICollection<Item> Items { get; } = new List<Item>();
    }
}

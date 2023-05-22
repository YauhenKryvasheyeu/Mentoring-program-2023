using CatalogService.DataAccess.Models;
using CatalogService.WebApi.Models;

namespace CatalogService.WebApi.Mappers
{
    public static class ItemExtension
    {
        public static ItemDto ToDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Description = item.Description,
                CategoryId = item.CategoryId
            };
        }

        public static Item ToDataAccess(this ItemDto item)
        {
            return new Item
            {
                Description = item.Description,
                CategoryId = item.CategoryId,
            };
        }
    }
}

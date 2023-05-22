using CatalogService.DataAccess.Models;
using CatalogService.WebApi.Models;

namespace CatalogService.WebApi.Mappers
{
    public static class CategoryExtension
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static Category ToDataAccess(this CategoryDto category)
        {
            return new Category
            {
                Name = category.Name
            };
        }
    }
}

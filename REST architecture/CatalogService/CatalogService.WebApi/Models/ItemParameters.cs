namespace CatalogService.WebApi.Models
{
    public class ItemParameters
    {
        private const int MaxPageSize = 5;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 2;

        public  int  CategoryId { get; set; }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}

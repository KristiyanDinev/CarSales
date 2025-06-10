using CarSales.Enums;

namespace CarSales.Models
{
    public class CarQueryParameters
    {
        public string? Make { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Year { get; set; }
        public CarSortEnum? SortBy { get; set; } 
        public bool SortDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

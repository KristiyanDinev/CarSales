using CarSales.Enums;

namespace CarSales.Models
{
    public class CarQueryParametersModel
    {
        public string? Make { get; set; }
        public decimal? Pirce { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public CarSortEnum? SortBy { get; set; } = CarSortEnum.CreatedAt;
        public bool SortDescending { get; set; } = true;
        public int Page { get; set; } = 1;
    }
}

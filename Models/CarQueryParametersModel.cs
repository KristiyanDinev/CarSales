using CarSales.Enums;

namespace CarSales.Models
{
    public class CarQueryParametersModel
    {
        // Search parameters
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }

        // sorting parameters
        public CarSortEnum? SortBy { get; set; } = CarSortEnum.CreatedAt;
        public bool SortDescending { get; set; } = true;


        // Pageination parameters
        public int Page { get; set; } = 1;
    }
}

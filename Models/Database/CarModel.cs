namespace CarSales.Models.Database
{
    public class CarModel
    {
        public int Id { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required decimal Price { get; set; }
        public required string Color { get; set; } 
        public required string Description { get; set; } 
        public required string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

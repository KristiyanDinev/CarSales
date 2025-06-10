using CarSales.Database;
using CarSales.Enums;
using CarSales.Models;
using CarSales.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Services
{
    public class CarService
    {
        private readonly DatabaseContext _databasesContext;

        public CarService(DatabaseContext databasesContext)
        {
            _databasesContext = databasesContext;
        }



        public async Task<List<CarModel>> GetCarsAsync(CarQueryParameters parameters)
        {
            IQueryable<CarModel> query = _databasesContext.Cars.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(parameters.Make)) {
                query = query.Where(c => c.Make == parameters.Make);
            }

            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue) {
                query = query.Where(c => c.Price <= parameters.MaxPrice.Value);
            }

            if (parameters.Year.HasValue)
            {
                query = query.Where(c => c.Year == parameters.Year.Value);
            }

            // Sorting
            query = parameters.SortBy switch
            {
                CarSortEnum.Price => parameters.SortDescending ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price),
                CarSortEnum.Year => parameters.SortDescending ? query.OrderByDescending(c => c.Year) : query.OrderBy(c => c.Year),
                CarSortEnum.CreatedAt => parameters.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                CarSortEnum.None => query.OrderByDescending(c => c.CreatedAt) // Default
,
                _ => throw new NotImplementedException()
            };

            // Paging
            query = query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

            return await query.ToListAsync();
        }
    }
}

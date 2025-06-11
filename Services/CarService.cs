using CarSales.Database;
using CarSales.Enums;
using CarSales.Models;
using CarSales.Models.Database;
using CarSales.Models.Forms;
using CarSales.Utilities;
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

        public async Task<bool> CreateCarAsync(CreateCarFormModel createCarForm)
        {
            if (createCarForm.Image == null)
            {
                return false;
            }

            string? imageUrl = await Utility.UploadCarImage(createCarForm.Image!);
            if (imageUrl == null)
            {
                return false;
            }

            CarModel car = new CarModel
            {
                Make = createCarForm.Make,
                Model = createCarForm.Model,
                Year = createCarForm.Year,
                Price = createCarForm.Price,
                Description = createCarForm.Description,
                Color = createCarForm.Color,
                ImageUrl = imageUrl,
            };

            await _databasesContext.Cars.AddAsync(car);
            return await _databasesContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditCarAsync(CreateCarFormModel createCarForm)
        {
            CarModel? car = await _databasesContext.Cars
                .FirstOrDefaultAsync(c => c.Id == createCarForm.Id);
            if (car == null) { 
                return false;
            }

            if (createCarForm.Image != null)
            {
                string? imageUrl = await Utility
                    .UpdateCarImage(car.ImageUrl, createCarForm.Image);
                if (imageUrl == null)
                {
                    return false;
                }
                car.ImageUrl = imageUrl;
            }

            car.Make = createCarForm.Make;
            car.Model = createCarForm.Model;
            car.Year = createCarForm.Year;
            car.Price = createCarForm.Price;
            car.Description = createCarForm.Description;
            car.Color = createCarForm.Color;

            _databasesContext.Cars.Update(car);
            return await _databasesContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteCarAsync(int id)
        {
            CarModel? car = await _databasesContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                return false;
            }

            _databasesContext.Cars.Remove(car);
            return await _databasesContext.SaveChangesAsync() > 0;
        }
    }
}

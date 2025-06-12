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


        public async Task<List<CarModel>> GetCarsAsync(CarQueryParametersModel parameters)
        {
            IQueryable<CarModel> query = _databasesContext.Cars.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(parameters.Make))
            {
                query = query.Where(c => c.Make.Equals(parameters.Make));
            }

            if (!string.IsNullOrEmpty(parameters.Model))
            {
                query = query.Where(c => c.Model == parameters.Model);
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

                _ => throw new ArgumentException()
            };

            return await Utility.GetPageAsync<CarModel>(query, parameters.Page);
        }

        public async Task<bool> CreateCarAsync(CreateCarFormModel createCarForm)
        {
            string? imageUrl = await Utility.UploadCarImage(createCarForm.Image!);
            if (imageUrl == null)
            {
                return false;
            }

            CarModel car = new CarModel
            {
                Make = createCarForm.Make.ToUpper(),
                Model = createCarForm.Model.ToUpper(),
                Year = createCarForm.Year,
                Price = createCarForm.Price,
                Description = createCarForm.Description,
                Color = createCarForm.Color.ToUpper(),
                ImageUrl = imageUrl,
            };

            await _databasesContext.Cars.AddAsync(car);
            return await _databasesContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditCarAsync(CreateCarFormModel createCarForm)
        {
            CarModel? car = await _databasesContext.Cars
                .FirstOrDefaultAsync(c => c.Id == createCarForm.Id);
            if (car == null)
            {
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

            car.Make = createCarForm.Make.ToUpper();
            car.Model = createCarForm.Model.ToUpper();
            car.Year = createCarForm.Year;
            car.Price = createCarForm.Price;
            car.Description = createCarForm.Description;
            car.Color = createCarForm.Color.ToUpper();

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

            if (!Utility.DeleteImage(car.ImageUrl)) 
            {
                return false;
            }

            _databasesContext.Cars.Remove(car);
            return await _databasesContext.SaveChangesAsync() > 0;
        }

        public async Task<CarModel?> GetCarByIdAsync(int id)
        {
            return await _databasesContext.Cars
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

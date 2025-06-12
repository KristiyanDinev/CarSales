using Microsoft.EntityFrameworkCore;

namespace CarSales.Utilities
{
    public class Utility
    {
        public static int pageSize = 10;
        public static string AdminRoleName = "Admin";

        public static async Task<string?> UploadCarImage(IFormFile Image)
        {
            if (Image == null)
            {
                return null;
            }

            string uploadsFolder = Path.Combine("wwwroot", "images", "cars");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}.png";
            string imagePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                using FileStream stream = new FileStream(imagePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                return $"/images/cars/{uniqueFileName}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string?> UpdateCarImage(string OldImage, IFormFile Image)
        {
            if (Image == null)
            {
                return null;
            }

            string uploadsFolder = Path.Combine("wwwroot", "images", "cars");
            string oldImagePath = Path.Combine(uploadsFolder, OldImage.TrimStart('/'));

            string uniqueFileName = $"{Guid.NewGuid()}.png";
            string imagePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }

                using FileStream stream = new FileStream(
                    Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create);

                await Image.CopyToAsync(stream);
                return $"/images/cars/{uniqueFileName}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool DeleteImage(string imagePath)
        {
            string fullPath = Path.Combine("wwwroot", imagePath.TrimStart('/'));
            try
            {
                if (Path.Exists(fullPath))
                {
                    // Delete the image file
                    File.Delete(fullPath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<List<T>> GetPageAsync<T>(IQueryable<T> query, int pageNumber)
        {
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarSales.Utilities
{
    public class Utility
    {
        public static void HandleModelErrors(IEnumerable<string> Errors,
           ref ModelStateDictionary ModelState)
        {
            foreach (string error in Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        public static async Task<string?> UploadCarImage(IFormFile Image)
        {
            if (Image == null || Image.Length == 0)
            {
                return null;
            }

            // wwwroot/images/cars/{carId}.png
            string[] files = Directory.GetFiles(Path.Combine("wwwroot", "images", "cars"));

            int imageId = 0;
            foreach (string fileName in files)
            {
                try
                {
                    int id = int.Parse(fileName.Split('.')[0]);
                    if (id > imageId)
                    {
                        imageId = id;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            imageId++;
            string imagePath = $"wwwroot/images/cars/{imageId}.png";
            string res = $"/images/cars/{imageId}.png";

            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                await Image.CopyToAsync(new FileStream(imagePath, FileMode.Create));
                return res;

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
            // wwwroot/images/users/{userId}.png
            string[] imagePathParts = OldImage.Split('.');

            if (!int.TryParse(imagePathParts[imagePathParts.Length - 2], out int imageId))
            {
                return null;
            }

            string imagePath = $"wwwroot/images/cars/{imageId}.png";
            string res = $"/images/cars/{imageId}.png";
            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);
                if (File.Exists(imagePath))
                {
                    // Delete the existing image file
                    File.Delete(imagePath);
                }
                await Image.CopyToAsync(new FileStream(imagePath, FileMode.Create));
                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookingWebApiTask.Application.Services
{
    public static class ImageServices
    {
        public static async Task<string> SaveImageAsync(IFormFile formFile, string rootPath)
        {
            if (formFile == null)
                return string.Empty;

            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            var imagePath = Path.Combine(rootPath, "images", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return $"images/{imageName}";
        }

        public static void DeleteImage(string imageUrl, string rootPath)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            // Construct the absolute path
            var imagePath = Path.Combine(rootPath, imageUrl);

            // Check if the file exists before attempting to delete
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}

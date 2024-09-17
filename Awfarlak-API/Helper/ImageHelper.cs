namespace Awfarlak_API.Helper
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const int _maxFileSizeBytes = 5 * 1024 * 1024;

        public ImageHelper(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid file");

            if (imageFile.Length > _maxFileSizeBytes)
                throw new ArgumentException("File size exceeds the limit");

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        public async Task DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;


            Uri uri;
            if (Uri.TryCreate(imagePath, UriKind.Absolute, out uri))
            {
                var relativePath = uri.AbsolutePath.TrimStart('/');
                var fullPath = Path.Combine(_hostEnvironment.WebRootPath, relativePath);

                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                }
            }
            else
            {
                throw new ArgumentException("Invalid image path format.");
            }
        }



        public async Task<string> UpdateImageAsync(IFormFile newImageFile, string existingImagePath)
        {
            await DeleteImageAsync(existingImagePath);
            return await SaveImageAsync(newImageFile);
        }
    }
}

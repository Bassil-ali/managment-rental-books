using Bookify.Web.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Bookify.Web.Services
{
	public class ImageService : IImageService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
		private int _maxAllowedSize = 2097152;

		public ImageService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail)
		{
			var extension = Path.GetExtension(image.FileName);

			if (!_allowedExtensions.Contains(extension))
				return (isUploaded: false, errorMessage: Errors.NotAllowedExtension);

			if (image.Length > _maxAllowedSize)
				return (isUploaded: false, errorMessage: Errors.MaxSize);

			// Ensure main directory exists
			var fullFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath.TrimStart('/'));
			if (!Directory.Exists(fullFolderPath))
				Directory.CreateDirectory(fullFolderPath);

			var path = Path.Combine(fullFolderPath, imageName);

			using (var stream = File.Create(path))
			{
				await image.CopyToAsync(stream);
			}

			if (hasThumbnail)
			{
				// Ensure thumbnail directory exists
				var thumbFolderPath = Path.Combine(fullFolderPath, "thumb");
				if (!Directory.Exists(thumbFolderPath))
					Directory.CreateDirectory(thumbFolderPath);

				var thumbPath = Path.Combine(thumbFolderPath, imageName);

				using var loadedImage = Image.Load(image.OpenReadStream());
				var ratio = (float)loadedImage.Width / 200;
				var height = loadedImage.Height / ratio;
				loadedImage.Mutate(i => i.Resize(width: 200, height: (int)height));
				loadedImage.Save(thumbPath);
			}

			return (isUploaded: true, errorMessage: null);
		}

		public void Delete(string imagePath, string? imageThumbnailPath = null)
		{
			var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));

			if (File.Exists(oldImagePath))
				File.Delete(oldImagePath);

			if (!string.IsNullOrEmpty(imageThumbnailPath))
			{
				var oldThumbPath = Path.Combine(_webHostEnvironment.WebRootPath, imageThumbnailPath.TrimStart('/'));
				if (File.Exists(oldThumbPath))
					File.Delete(oldThumbPath);
			}
		}

	}
}
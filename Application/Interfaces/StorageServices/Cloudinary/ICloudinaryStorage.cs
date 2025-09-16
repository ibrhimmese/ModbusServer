using Microsoft.AspNetCore.Http;

namespace Application.StorageInterfaces.Cloudinary;

public interface ICloudinaryStorage
{
    Task<(string publicId, string url)> UploadAsync(IFormFile file, string folderName);
    Task DeleteAsync(string publicId);
    
}

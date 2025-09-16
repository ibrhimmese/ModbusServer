using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Application.StorageInterfaces.Cloudinary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructure.StorageConcretes.CloudinaryStorage;



public class CloudinaryStorage : ICloudinaryStorage
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryStorage(IConfiguration configuration)
    {
        var cloudinaryConfig = configuration.GetSection("Cloudinary");
        var account = new Account(
            cloudinaryConfig["CloudName"],
            cloudinaryConfig["ApiKey"],
            cloudinaryConfig["ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<(string publicId, string url)> UploadAsync(IFormFile file, string folderName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folderName
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return (uploadResult.PublicId, uploadResult.SecureUrl.ToString());
    }

    public async Task DeleteAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}


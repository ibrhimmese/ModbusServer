using Application.ExceptionTypes;
using Application.StorageInterfaces.GoogleCloud;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.StorageConcretes.Google;

public class GoogleStorage : Storage, IGoogleStorage
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleStorage(IConfiguration configuration)
    {
        _storageClient = StorageClient.Create();
        _bucketName = configuration["Google:BucketName"] ?? throw new OperationException("ContainerName is not configured.");
    }

    public async Task DeleteAsync(string pathOrContainerName, string fileName)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, $"{pathOrContainerName}/{fileName}");
    }

    public List<string> GetFiles(string pathOrContainerName)
    {
        var objects = _storageClient.ListObjects(_bucketName, pathOrContainerName);
        return objects.Select(o => o.Name).ToList();
    }

    public bool CheckFileExists(string pathOrContainerName, string fileName)
    {
        var objects = _storageClient.ListObjects(_bucketName, $"{pathOrContainerName}/{fileName}");
        return objects.Any();
    }

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
    {
        List<(string fileName, string pathOrContainerName)> datas = new();
        foreach (IFormFile file in files)
        {
            string fileNewName = await FileRenameAsync(pathOrContainerName, file.FileName, CheckFileExists);

            using (var stream = file.OpenReadStream())
            {
                await _storageClient.UploadObjectAsync(_bucketName, $"{pathOrContainerName}/{fileNewName}", file.ContentType, stream);
            }

            datas.Add((fileNewName, pathOrContainerName));
        }

        return datas;
    }
}


using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Application.ExceptionTypes;
using Application.StorageInterfaces.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.StorageConcretes.Azure;

public class AzureStorage : Storage, IAzureStorage
{

    private readonly BlobServiceClient _blobServiceClient;
    BlobContainerClient? _blobContainerClient;

    public AzureStorage(IConfiguration configuration)
    {
        _blobServiceClient = new(configuration["Storage:Azure"] ?? throw new OperationException("ContainerName is not configured."));
    }

    public async Task DeleteAsync(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.DeleteAsync();
    }

    public List<string> GetFiles(string containerName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
    }

    public bool CheckFileExists(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
    }

    //public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
    //{
    //    _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
    //    await _blobContainerClient.CreateIfNotExistsAsync();
    //    await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

    //    List<(string fileName, string pathOrContainerName)> datas = new();
    //    foreach (IFormFile file in files)
    //    {
    //       string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);

    //       BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName); //
    //        await blobClient.UploadAsync(file.OpenReadStream());
    //        datas.Add((fileNewName, containerName));
    //    }
    //    return datas;
    //}

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await _blobContainerClient.CreateIfNotExistsAsync();
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

        List<(string fileName, string pathOrContainerName)> datas = new();
        foreach (IFormFile file in files)
        {
            // Dosya adını yeniden adlandırın
            string fileNewName = await FileRenameAsync(containerName, file.FileName, CheckFileExists);

            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);

            // Dosyayı blob storage'a yükleyin
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true); // true parametresi mevcut blob'u üzerine yazar
            }

            datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
        }
        return datas;
    }
}


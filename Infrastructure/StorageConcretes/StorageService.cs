using Application.StorageInterfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.StorageConcretes;

public class StorageService : IStorageService
{
    private readonly IStorage _storage;

    public StorageService(IStorage storage)
    {
        _storage = storage;
    }

    public string StorageName { get => _storage.GetType().Name; }

    public async Task DeleteAsync(string pathOrContainerName, string fileName)
        => await _storage.DeleteAsync(pathOrContainerName, fileName);


    public List<string> GetFiles(string pathOrContainerName)
        => _storage.GetFiles(pathOrContainerName);


    public bool CheckFileExists(string pathOrContainerName, string fileName)
        => _storage.CheckFileExists(pathOrContainerName, fileName);

    public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        => _storage.UploadAsync(pathOrContainerName, files);
}

using Microsoft.AspNetCore.Http;

namespace Application.StorageInterfaces;

public interface IStorage
{
    Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);

    Task DeleteAsync(string pathOrContainerName, string fileName);

    List<string> GetFiles(string pathOrContainerName);

    bool CheckFileExists(string pathOrContainerName, string fileName);
}

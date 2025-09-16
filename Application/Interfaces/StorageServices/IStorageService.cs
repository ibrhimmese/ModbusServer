namespace Application.StorageInterfaces;

public interface IStorageService : IStorage
{
    public string StorageName { get; }
}

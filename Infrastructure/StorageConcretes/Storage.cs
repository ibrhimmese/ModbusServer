using Infrastructure.StorageConcretes.StaticServices;

namespace Infrastructure.StorageConcretes;

public class Storage
{
    protected delegate bool HasFile(string pathOrContainerName, string fileName);

    protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod)
    {
        string extension = Path.GetExtension(fileName);
        string fileNameWithoutExtension = NameService.CharacterRegulatory(Path.GetFileNameWithoutExtension(fileName));
        string newFileName = $"{fileNameWithoutExtension}{extension}";

        int counter = 1;
        while (hasFileMethod(pathOrContainerName, newFileName))
        {
            newFileName = $"{fileNameWithoutExtension}-{counter++}{extension}";
        }

        return await Task.FromResult(newFileName);
    }
}

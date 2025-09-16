using Application.StorageInterfaces.Local;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace Infrastructure.StorageConcretes.Local;

public class LocalStorage : Storage  , ILocalStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public LocalStorage(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }


    public async Task DeleteAsync(string path, string fileName)

       => await Task.Run(() =>File.Delete($"{path}\\{fileName}"));




    public List<string> GetFiles(string path)
    {
        DirectoryInfo directory = new(path);
        return directory.GetFiles().Select(f => f.Name).ToList();
    }


    public bool CheckFileExists(string path, string fileName)
       => File.Exists($"{path}\\{fileName}");


    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
    {
        // Tam yükleme yolunu oluşturun
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

        // Yükleme dizinini oluşturun (eğer yoksa)
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        List<(string fileName, string pathOrContainerName)> datas = new();

        foreach (IFormFile file in files)
        {
            // Dosya adını yeniden adlandırın
            string fileNewName = await FileRenameAsync(uploadPath, file.FileName, CheckFileExists);

            // Tam dosya yolunu oluşturun
            string fullPath = Path.Combine(uploadPath, fileNewName);

            // Dosyayı kopyalayın
            await CopyFileAsync(fullPath, file);

            // Sonuç listesine dosya adı ve yolu ekleyin
            datas.Add((fileNewName, Path.Combine(path, fileNewName)));
        }

        return datas;
    }



    private async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            return true;
        }
        catch (Exception)
        {
            //todo log!
            throw;
        }

    }
}

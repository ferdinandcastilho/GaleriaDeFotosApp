using System.Security.Cryptography;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.Core.Services;

public class FotosDataService : IFotosDataService
{
    public async Task<IEnumerable<Foto>> GetPhotos()
    {
        var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        await Task.CompletedTask;

        return Directory.GetFiles(imagePath)
            .Where(file => Path.GetExtension(file) is ".png" or ".jpg").Select(file =>
                new Foto { ImageId = CreateHash(file), ImageUri = new Uri(file) }).ToList();
    }

    private static string CreateHash(string file)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(file);
        var byteArrayHash = md5.ComputeHash(stream);
        return System.Text.Encoding.UTF8.GetString(byteArrayHash);
    }
}
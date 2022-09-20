using System.Diagnostics;
using System.Security.Cryptography;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using HashidsNet;

namespace GaleriaDeFotos.Core.Services;

public class FotosDataService : IFotosDataService
{
    private const string Salt = "MaikeuFernando";

    #region IFotosDataService Members

    public async Task<IEnumerable<Foto>> GetPhotosAsync()
    {
        var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        await Task.CompletedTask;
        var files = Directory.GetFiles(imagePath)
            .Where(file => Path.GetExtension(file) is ".png" or ".jpg");
        var photos = new List<Foto>();
        foreach (var file in files)
        {
            var photo = await AddPhoto(file);
            photos.Add(photo);
        }
#if DEBUG
        await using var dataContext = new FotoContext();

        foreach (var cat in dataContext.Fotos.ToList())
            Debug.WriteLine($"Id= {cat.ImageId}, Uri = {cat.ImageUri}");
#endif

        return photos;
    }

    #endregion

    private async Task<Foto> AddPhoto(string file)
    {
        var hash = CreateHash(file);
        var photo = new Foto { ImageId = hash, ImageUri = new Uri(file) };
        await using var dataContext = new FotoContext();
        dataContext.Fotos.Add(photo.ToData());
        await dataContext.SaveChangesAsync();

        return photo;
    }

    private static string CreateHash(string file)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(file);
        var byteArrayHash = md5.ComputeHash(stream);
        var hashids = new Hashids(Salt);
        var bytesAsInts = byteArrayHash.Select(x => (int)x).ToArray();
        return hashids.Encode(bytesAsInts);
    }
}
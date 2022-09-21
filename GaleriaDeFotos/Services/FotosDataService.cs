using System.Diagnostics;
using System.Security.Cryptography;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using HashidsNet;

namespace GaleriaDeFotos.Services;

public class FotosDataService : IFotosDataService
{
    private const string Salt = "MaikeuFernando";

    #region IFotosDataService Members

    public async Task<IEnumerable<Foto>> GetPhotosAsync()
    {
        var photos = new List<Foto>();
        var fotoContext = App.GetService<FotoContext>();
        if (fotoContext.Fotos == null) return photos;
        if (!fotoContext.Fotos.Any())
        {
            var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            await Task.CompletedTask;
            var files = Directory.GetFiles(imagePath)
                .Where(file => Path.GetExtension(file) is ".png" or ".jpg");

            foreach (var file in files)
            {
                var photo = await AddPhoto(file, fotoContext);
                photos.Add(photo);
            }
        } else
        {
            photos.AddRange(fotoContext.Fotos.Select(foto => new Foto(foto)));
        }

#if DEBUG

        foreach (var cat in fotoContext.Fotos.ToList())
            Debug.WriteLine($"Id= {cat.ImageId}, Uri = {cat.ImageUri}");
#endif
        return photos;
    }

    public async void SetFavorite(Foto foto, bool isFavorite)
    {
        var fotoContext = App.GetService<FotoContext>();
        var fotoData = fotoContext.Fotos.First(data => data.ImageId == foto.ImageId);
        fotoData.IsFavorite = isFavorite;
        await fotoContext.SaveChangesAsync();
    }

    #endregion

    private async Task<Foto> AddPhoto(string file, FotoContext context)
    {
        var hash = CreateHash(file);
        var photo = new Foto { ImageId = hash, ImageUri = new Uri(file) };
        context.Fotos.Add(photo.ToData());
        await context.SaveChangesAsync();

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
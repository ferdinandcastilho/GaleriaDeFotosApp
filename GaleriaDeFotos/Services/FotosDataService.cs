using System.Diagnostics;
using System.Security.Cryptography;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using HashidsNet;

namespace GaleriaDeFotos.Services;

public class FotosDataService : IFotosDataService
{
    private const string Salt = "MaikeuFernando";
    private readonly FotoContext _fotoContext;

    #region IFotosDataService Members

    public FotosDataService(FotoContext fotoContext)
    {
        _fotoContext = fotoContext;
    }

    public async Task<IEnumerable<Foto>> GetPhotosAsync(string imagePath = null)
    {
        var photos = new List<Foto>();
        if (!_fotoContext.Fotos.Any())
        {
            if (imagePath is null) imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); // Substituir SpecialFolder por última pasta declarada pelo usuário

            await Task.CompletedTask;

            var files = Directory.GetFiles(imagePath)
                .Where(file => Path.GetExtension(file) is ".png" or ".jpg");

            foreach (var file in files)
            {
                var photo = await AddPhoto(file);
                photos.Add(photo);
            }
        }
        else
        {
            photos.AddRange(_fotoContext.Fotos.Select(foto => new Foto(foto)));
        }

#if DEBUG

        foreach (var cat in _fotoContext.Fotos.ToList())
            Debug.WriteLine($"Id= {cat.ImageId}, Uri = {cat.ImageUri}");
#endif
        return photos;
    }

    public async void SetFavorite(Foto foto, bool isFavorite)
    {
        var fotoData = _fotoContext.Fotos.First(data => data.ImageId == foto.ImageId);
        fotoData.IsFavorite = isFavorite;
        await _fotoContext.SaveChangesAsync();
    }

    #endregion

    private async Task<Foto> AddPhoto(string file)
    {
        var hash = CreateHash(file);
        var photo = new Foto { ImageId = hash, ImageUri = new Uri(file) };
        _fotoContext.Fotos.Add(photo.ToData());
        await _fotoContext.SaveChangesAsync();

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
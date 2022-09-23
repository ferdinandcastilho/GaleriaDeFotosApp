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

    public FotosDataService(FotoContext fotoContext) { _fotoContext = fotoContext; }

    public async Task<IEnumerable<Foto>> GetPhotosAsync(string? imagePath = null)
    {
        imagePath ??= Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        await Task.CompletedTask;

        var files = Directory.GetFiles(imagePath)
            .Where(file => Path.GetExtension(file) is ".png" or ".jpg");
        _fotoContext.Clear();
        await _fotoContext.SaveChangesAsync();
        var photos = await SetupPhotosAsync(files);


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

    private async Task<List<Foto>> SetupPhotosAsync(IEnumerable<string> files)
    {
        var list = new List<Foto>();
        foreach (var file in files)
        {
            var hash = CreateHash(file);
            if (list.Find(foto => foto.ImageId == hash) != null) continue;
            list.Add(new Foto { ImageId = hash, ImageUri = new Uri(file) });
        }

        await _fotoContext.Fotos.AddRangeAsync(list.Select(foto => foto.ToData()).ToList());
        await _fotoContext.SaveChangesAsync();

        return list;
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
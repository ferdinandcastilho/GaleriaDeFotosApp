using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using HashidsNet;

namespace GaleriaDeFotos.Services;

public class FotosDataService : IFotosDataService
{
    private const string Salt = "MaikeuFernando";
    private readonly FotoContext _fotoContext;
    private string _lastPath = string.Empty;

    #region IFotosDataService Members

    public FotosDataService(FotoContext fotoContext)
    {
        _fotoContext = fotoContext;
    }

    public async Task<IEnumerable<string>> GetImagesFromFolderAsync(string? imagePath = null)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            imagePath = string.IsNullOrWhiteSpace(_lastPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                : _lastPath;
        }

        _lastPath = imagePath;

        await Task.CompletedTask;

        var files = Directory.GetFiles(imagePath)
            .Where(file => Path.GetExtension(file) is ".png" or ".jpg");

        return files;

    }

    public async Task<IEnumerable<Foto>> GetPhotosAsync(string? imagePath = null)
    {
        var files = await GetImagesFromFolderAsync(imagePath);
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
        var listToAdd = new List<FotoData>();
        var retList = new List<Foto>();
        foreach (var file in files)
        {
            var hash = CreateHash(file);
            var item = new Foto { ImageId = hash, ImageUri = new Uri(file) };
            if (retList.Find(foto => foto.ImageId == item.ImageId) != null) continue;
            retList.Add(item);
            if (await _fotoContext.Fotos.FindAsync(hash) != null) continue;
            listToAdd.Add(item.ToData());
        }

        await _fotoContext.Fotos.AddRangeAsync(listToAdd);
        await _fotoContext.SaveChangesAsync();
        return retList;
    }

    public IEnumerable<Foto> Select(Expression<Func<FotoData, bool>> predicate)
    {
        var fotoDatas = _fotoContext.Fotos.Where(predicate);
        return fotoDatas.Select(foto => new Foto(foto));
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
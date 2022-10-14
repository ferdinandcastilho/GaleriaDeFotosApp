using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.DataContext;
using GaleriaDeFotos.Core.Models;
using HashidsNet;

namespace GaleriaDeFotos.Services;

public class FotosDataService : IFotosDataService
{
    private readonly FotoContext? _fotoContext;
    private string _lastPath = string.Empty;

    public FotosDataService(FotoContext? fotoContext) { _fotoContext = fotoContext; }

    #region IFotosDataService Members

    public async Task<IEnumerable<Foto>> GetPhotosAsync(string? imagePath = null)
    {
        imagePath ??= _lastPath;
        var files = await GetImagesFromFolderAsync(imagePath);
        if (_fotoContext == null) return new List<Foto>();
        var photos = await SetupPhotosAsync(files);
#if DEBUG
        foreach (var cat in _fotoContext.Fotos.ToList())
            Debug.WriteLine($"Id= {cat.ImageId}, Uri = {cat.ImageUri}");
#endif
        return photos;
    }

    public async void SetFavorite(Foto foto, bool isFavorite)
    {
        if (_fotoContext == null) return;
        var fotoData = _fotoContext.Fotos.First(data => data.ImageId == foto.ImageId);
        fotoData.IsFavorite = isFavorite;
        await _fotoContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Foto>> SelectAsync(Expression<Func<FotoData, bool>> predicate)
    {
        if (_fotoContext == null) return new List<Foto>();
        await GetPhotosAsync();
        var fotoDatas = _fotoContext.Fotos.Where(predicate);
        return fotoDatas.Select(foto => new Foto(foto));
    }

    #endregion

    public async Task<IEnumerable<string>> GetImagesFromFolderAsync(string? imagePath = null)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            imagePath = string.IsNullOrWhiteSpace(_lastPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                : _lastPath;

        _lastPath = imagePath;

        await Task.CompletedTask;

        var files = Directory.GetFiles(imagePath)
            .Where(file => Path.GetExtension(file) is ".png" or ".jpg");

        return files;
    }

    private async Task<List<Foto>> SetupPhotosAsync(IEnumerable<string> files)
    {
        var listToAdd = new List<FotoData>();
        var retList = new List<Foto>();
        if (_fotoContext == null) return retList;
        foreach (var file in files)
        {
            var hash = CreateHash(file);
            var item = new Foto { ImageId = hash, ImageUri = new Uri(file) };
            if (retList.Exists(foto => foto.ImageId == item.ImageId)) continue;
            retList.Add(item);
            if (await _fotoContext.Fotos.FindAsync(item.ImageId) != null) continue;
            listToAdd.Add(item.ToData());
        }

        //Remove os que não estão mais na pasta
        foreach (var foto in _fotoContext.Fotos)
            if (!retList.Exists(f => f.ImageId == foto.ImageId))
                _fotoContext.Fotos.Remove(foto);

        await _fotoContext.Fotos.AddRangeAsync(listToAdd);
        await _fotoContext.SaveChangesAsync();
        return retList;
    }

    private static string CreateHash(string file)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(file);
        var byteArrayHash = md5.ComputeHash(stream);
        var filename = Path.GetFileName(file);
        var hashids = new Hashids(filename);
        var bytesAsInts = byteArrayHash.Select(x => (int)x).ToArray();
        return hashids.Encode(bytesAsInts);
    }
}
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.Core.Services;

public class FotosDataService : IFotosDataService
{
    public async Task<IEnumerable<Foto>> GetPhotos()
    {
        var list = new List<Foto>();
        var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        await Task.CompletedTask;

        return Directory.GetFiles(imagePath)
            .Where(file => Path.GetExtension(file) is ".png" or ".jpg")
            .Select(file => new Foto { ImageId = list.Count, ImageUri = new Uri(file) })
            .ToList();
    }
}
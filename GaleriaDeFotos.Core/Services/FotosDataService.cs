using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.Core.Services;

public class FotosDataService : IFotosDataService
{
    public async Task<IEnumerable<Foto>> GetPhotos()
    {
        var list = new List<Foto>();
        var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        var files = Directory.GetFiles(imagePath);
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file);
            if (extension is ".png" or ".jpg")
            {
                var uri = new Uri(file);
                var photo = new Foto { ImageId = list.Count, ImageUri = uri };
                list.Add(photo);
            }
        }


        await Task.CompletedTask;
        return list;
    }
}
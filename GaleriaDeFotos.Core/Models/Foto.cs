using CommunityToolkit.Mvvm.ComponentModel;

namespace GaleriaDeFotos.Core.Models;

public partial class Foto : ObservableObject
{
    [ObservableProperty] private string _imageId;

    [ObservableProperty] private Uri _imageUri;
    [ObservableProperty] private bool _isFavorite;
    public Foto() { }

    public Foto(FotoData data) { FromData(data); }

    public FotoData ToData()
    {
        var data = new FotoData
        {
            ImageId = ImageId, ImageUri = ImageUri.LocalPath, IsFavorite = IsFavorite
        };
        return data;
    }

    public void FromData(FotoData data)
    {
        ImageId = data.ImageId;
        ImageUri = new Uri(data.ImageUri);
        IsFavorite = data.IsFavorite;
    }
}
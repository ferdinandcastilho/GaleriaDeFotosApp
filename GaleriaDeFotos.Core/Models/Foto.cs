using CommunityToolkit.Mvvm.ComponentModel;

namespace GaleriaDeFotos.Core.Models;

public partial class Foto : ObservableObject
{
    public const double StartWidth = 320;
    public const double MinWidth = 100;
    public const double MaxWidth = 400;
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

    public static double GetStartSliderWidth()
    {
        var widthRange = MaxWidth - MinWidth;
        return ((StartWidth - MinWidth) / widthRange) * 100;
    }

    public static double GetUpdatedWidth(double percent)
    {
        var widthRange = MaxWidth - MinWidth;
        var percentValue = percent / 100;
        return MinWidth + widthRange * percentValue;
    }
}
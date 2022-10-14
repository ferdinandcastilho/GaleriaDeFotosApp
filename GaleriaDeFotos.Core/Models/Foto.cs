using CommunityToolkit.Mvvm.ComponentModel;

namespace GaleriaDeFotos.Core.Models;

/// <inheritdoc />
/// <summary>
///     Imagem
/// </summary>
public partial class Foto : ObservableObject
{
    /// <summary>
    ///     Largura Inicial
    /// </summary>
    public const double StartWidth = 320;

    /// <summary>
    ///     Largura Mínima
    /// </summary>
    public const double MinWidth = 100;

    /// <summary>
    ///     Largura Máxima
    /// </summary>
    public const double MaxWidth = 400;

    /// <summary>
    ///     Pasta em que a Imagem se Encontra
    /// </summary>
    [ObservableProperty] private string _folder;

    /// <summary>
    ///     Id Único da Imagem
    /// </summary>
    [NotifyPropertyChangedFor(nameof(ImageUri))] [ObservableProperty]
    private string _imageId;

    /// <summary>
    ///     Endereço da Imagem
    /// </summary>
    [ObservableProperty] private Uri _imageUri;

    /// <summary>
    ///     Foto é Favorita?
    /// </summary>
    [ObservableProperty] private bool _isFavorite;

    public Foto() { }

    public Foto(FotoData data) { FromData(data); }

    /// <summary>
    ///     Se converte para <see cref="FotoData" />
    /// </summary>
    /// <returns><see cref="FotoData" /> Convertida</returns>
    public FotoData ToData()
    {
        var data = new FotoData
        {
            ImageId = ImageId,
            ImageUri = ImageUri.LocalPath,
            IsFavorite = IsFavorite,
            Folder = Folder
        };
        return data;
    }

    /// <summary>
    ///     Se monta a partir de uma <see cref="FotoData" />
    /// </summary>
    /// <param name="data"><see cref="FotoData" /> a ser usada</param>
    public void FromData(FotoData data)
    {
        ImageId = data.ImageId;
        ImageUri = new Uri(data.ImageUri);
        Folder = data.Folder;
        IsFavorite = data.IsFavorite;
    }

    /// <summary>
    ///     Obtém largura Inicial do Slider
    /// </summary>
    /// <returns>Largura Inicial do Slider</returns>
    public static double GetStartSliderWidth()
    {
        var widthRange = MaxWidth - MinWidth;
        return (StartWidth - MinWidth) / widthRange * 100;
    }

    /// <summary>
    ///     Obtém Largura Atualizada
    /// </summary>
    /// <param name="percent">Porcentagem para ser convertida em largura do slider</param>
    /// <returns>Largura do Slider de Tamanho</returns>
    public static double GetUpdatedWidth(double percent)
    {
        var widthRange = MaxWidth - MinWidth;
        var percentValue = percent / 100;
        return MinWidth + widthRange * percentValue;
    }
}
using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Models;

/// <summary>
///     Parâmetros para passar para a FullViewModel
/// </summary>
public class FotoParameters
{
    public readonly BaseFotosViewModel BaseFotosViewModel;
    public readonly string ImageId;

    public FotoParameters(string imageId, BaseFotosViewModel baseFotosViewModel)
    {
        ImageId = imageId;
        BaseFotosViewModel = baseFotosViewModel;
    }
}
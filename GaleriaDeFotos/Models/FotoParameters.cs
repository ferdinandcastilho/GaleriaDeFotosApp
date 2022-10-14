using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Models;

public class FotoParameters
{
    public string ImageId;
    public BaseFotosViewModel BaseFotosViewModel;

    public FotoParameters(string imageId, BaseFotosViewModel baseFotosViewModel)
    {
        ImageId = imageId;
        BaseFotosViewModel = baseFotosViewModel;
    }
}
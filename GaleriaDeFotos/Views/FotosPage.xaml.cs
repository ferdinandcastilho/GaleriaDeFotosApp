using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosPage
{
    public FotosPage()
    {
        ViewModel = App.GetService<FotosViewModel>();
        InitializeComponent();
    }

    public FotosViewModel ViewModel { get; }
}
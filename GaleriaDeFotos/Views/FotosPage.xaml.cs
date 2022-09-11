using GaleriaDeFotos.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosPage : Page
{
    public FotosViewModel ViewModel
    {
        get;
    }

    public FotosPage()
    {
        ViewModel = App.GetService<FotosViewModel>();
        InitializeComponent();
    }
}

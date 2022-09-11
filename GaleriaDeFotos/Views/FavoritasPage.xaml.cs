using GaleriaDeFotos.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Views;

public sealed partial class FavoritasPage : Page
{
    public FavoritasViewModel ViewModel
    {
        get;
    }

    public FavoritasPage()
    {
        ViewModel = App.GetService<FavoritasViewModel>();
        InitializeComponent();
    }
}

using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Views;

public sealed partial class FavoritasPage : Page
{
    public FavoritasPage()
    {
        ViewModel = App.GetService<FavoritasViewModel>();
        InitializeComponent();
    }

    public FavoritasViewModel ViewModel { get; }
}
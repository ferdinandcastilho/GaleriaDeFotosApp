using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class FavoritasPage
{
    public FavoritasPage()
    {
        ViewModel = App.GetService<FavoritasViewModel>();
        InitializeComponent();
    }

    public FavoritasViewModel ViewModel { get; }
}
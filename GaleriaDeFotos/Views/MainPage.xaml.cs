using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class MainPage
{
    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    public MainViewModel ViewModel { get; }
}
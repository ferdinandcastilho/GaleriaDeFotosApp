using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

/// <inheritdoc cref="Microsoft.UI.Xaml.Controls.Page" />
/// <summary>
///     Página de Entrada
/// </summary>
public sealed partial class MainPage
{
    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    public MainViewModel ViewModel { get; }
}
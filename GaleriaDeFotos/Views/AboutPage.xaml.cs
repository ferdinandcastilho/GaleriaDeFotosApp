using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

/// <inheritdoc cref="Microsoft.UI.Xaml.Controls.Page" />
/// <summary>
///     Página de Sobre
/// </summary>
public sealed partial class AboutPage
{
    public AboutPage()
    {
        ViewModel = App.GetService<AboutViewModel>();
        InitializeComponent();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public AboutViewModel ViewModel { get; }
}
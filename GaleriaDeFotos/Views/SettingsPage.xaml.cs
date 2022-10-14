using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

/// <inheritdoc cref="Microsoft.UI.Xaml.Controls.Page" />
/// <summary>
///     Página de Opções
/// </summary>
public sealed partial class SettingsPage
{
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    public SettingsViewModel ViewModel { get; }
}
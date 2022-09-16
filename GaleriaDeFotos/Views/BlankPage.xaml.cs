using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class BlankPage
{
    public BlankPage()
    {
        ViewModel = App.GetService<BlankViewModel>();
        InitializeComponent();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public BlankViewModel ViewModel { get; }
}
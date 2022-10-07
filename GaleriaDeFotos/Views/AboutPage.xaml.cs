using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class AboutPage
{
    public AboutPage()
    {
        ViewModel = App.GetService<AboutViewModel>();
        InitializeComponent();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public AboutViewModel ViewModel
    {
        get;
    }
}
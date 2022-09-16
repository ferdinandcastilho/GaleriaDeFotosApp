using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosFullPage
{
    public FotosFullPage()
    {
        ViewModel = App.GetService<FotosFullViewModel>();
        InitializeComponent();
    }

    public FotosFullViewModel ViewModel { get; }
}
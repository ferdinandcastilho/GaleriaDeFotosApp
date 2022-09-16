using GaleriaDeFotos.ViewModels;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosDetailPage
{
    public FotosDetailPage()
    {
        ViewModel = App.GetService<FotosDetailViewModel>();
        InitializeComponent();
    }

    public FotosDetailViewModel ViewModel { get; }
}
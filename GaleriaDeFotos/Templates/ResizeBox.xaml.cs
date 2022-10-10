using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Templates;

public sealed partial class ResizeBox
{
    public ResizeBox()
    {
        ViewModel = App.GetService<FotosFullViewModel>();
        InitializeComponent();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FotosFullViewModel ViewModel { get; }

    private void ImageWidthOnChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.UpdateResizeHeight();
    }
}
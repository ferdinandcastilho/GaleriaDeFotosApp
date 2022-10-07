using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosPage
{
    public FotosPage()
    {
        ViewModel = App.GetService<FotosViewModel>();
        InitializeComponent();
        SizeSlider.Value = Foto.GetStartSliderWidth();
        AdaptiveGridView.ItemHeight = Foto.StartWidth * 9 / 16;
        AdaptiveGridView.DesiredWidth = Foto.StartWidth;
    }

    public FotosViewModel ViewModel { get; }

    private void SizeSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var width = Foto.GetUpdatedWidth(e.NewValue);
        AdaptiveGridView.ItemHeight = width * 9 / 16;
        AdaptiveGridView.DesiredWidth = width;
    }
}
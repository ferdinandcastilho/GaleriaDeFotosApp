using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace GaleriaDeFotos.Views;

public sealed partial class FavoritasPage
{
    public FavoritasPage()
    {
        ViewModel = App.GetService<FavoritasViewModel>();
        InitializeComponent();
        SizeSlider.Value = Foto.GetStartSliderWidth();
        AdaptiveGridView.ItemHeight = Foto.StartWidth * 9 / 16;
        AdaptiveGridView.DesiredWidth = Foto.StartWidth;
    }

    public FavoritasViewModel ViewModel { get; }

    private void SizeSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var width = Foto.GetUpdatedWidth(e.NewValue);
        AdaptiveGridView.ItemHeight = width * 9 / 16;
        AdaptiveGridView.DesiredWidth = width;
    }
}
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace GaleriaDeFotos.Views;

/// <inheritdoc cref="Microsoft.UI.Xaml.Controls.Page" />
/// <summary>
///     Página de Favoritos
/// </summary>
public sealed partial class FavoritasPage
{
    public FavoritasPage()
    {
        ViewModel = App.GetService<FavoritasViewModel>();
        InitializeComponent();
        SizeSlider.Value = Foto.GetStartSliderWidth();
        AdaptiveGridView.ItemHeight = Foto.StartWidth * 3 / 4;
        AdaptiveGridView.DesiredWidth = Foto.StartWidth;
    }

    public FavoritasViewModel ViewModel { get; }

    /// <summary>
    ///     Executada quando o valor do Slider de Tamanho é Modificado
    /// </summary>
    /// <param name="sender">Não Utilizado</param>
    /// <param name="e">Não Utilizado</param>
    private void SizeSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var width = Foto.GetUpdatedWidth(e.NewValue);
        AdaptiveGridView.ItemHeight = width * 3 / 4;
        AdaptiveGridView.DesiredWidth = width;
    }
}
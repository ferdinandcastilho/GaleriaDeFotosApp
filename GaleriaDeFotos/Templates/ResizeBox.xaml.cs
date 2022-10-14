using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Templates;

/// <inheritdoc cref="Microsoft.UI.Xaml.Controls.ContentDialog" />
/// <summary>
///     Caixa de diálogo para modificar o tamanho da Imagem
/// </summary>
public sealed partial class ResizeBox
{
    public ResizeBox()
    {
        ViewModel = App.GetService<FotosFullViewModel>();
        InitializeComponent();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FotosFullViewModel ViewModel { get; }

    /// <summary>
    ///     Executada para atualizar a altura da imagem enquanto se modifica a largura
    /// </summary>
    /// <param name="sender">Não Utilizado</param>
    /// <param name="e">Não Utilizado</param>
    private void ImageWidthOnChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.UpdateResizeHeight();
    }
}
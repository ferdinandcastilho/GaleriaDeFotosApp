using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Helpers;

/// <summary>
///     Extensões para o Frame, veja <see cref="Frame" />
/// </summary>
public static class FrameExtensions
{
    /// <summary>
    ///     Obtém a ViewModel da Página
    /// </summary>
    /// <param name="frame">Frame responsável</param>
    /// <returns>A ViewModel da Página</returns>
    public static object? GetPageViewModel(this Frame frame)
    {
        return frame.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
    }
}
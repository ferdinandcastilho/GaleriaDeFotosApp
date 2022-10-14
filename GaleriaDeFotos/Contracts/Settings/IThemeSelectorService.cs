using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.Contracts.Settings;

/// <summary>
///     Interface do Serviço de Seleção de Temas
/// </summary>
public interface IThemeSelectorService
{
    /// <summary>
    ///     Obtém o Tema Selecionado
    /// </summary>
    ElementTheme Setting { get; }

    /// <summary>
    ///     Inicializa o Serviço
    /// </summary>
    /// <returns>Task para Async</returns>
    Task InitializeAsync();

    /// <summary>
    ///     Seta o Tema Desejado
    /// </summary>
    /// <param name="theme">Tema Desejado</param>
    /// <returns>Task para Async</returns>
    Task SetThemeAsync(ElementTheme theme);

    /// <summary>
    ///     Executa o necessário para a seleção do Tema
    /// </summary>
    /// <returns>Task para Async</returns>
    Task SetRequestedThemeAsync();
}
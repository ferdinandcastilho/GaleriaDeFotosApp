using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Contracts.Services;

/// <summary>
///     Interface para o Serviço responsável pela Navigation View
/// </summary>
public interface INavigationViewService
{
    /// <summary>
    ///     Itens do Menu
    /// </summary>
    IList<object>? MenuItems { get; }

    /// <summary>
    ///     Item do Menu Responsável pelas Configurações
    /// </summary>
    object? SettingsItem { get; }

    /// <summary>
    ///     Inicializa o Serviço
    /// </summary>
    /// <param name="navigationView">View responsável pelo serviço</param>
    void Initialize(NavigationView navigationView);

    /// <summary>
    ///     Remover registros dos eventos
    /// </summary>
    void UnregisterEvents();

    /// <summary>
    ///     Obtém o item que está selecionado
    /// </summary>
    /// <param name="pageType">Tipo de página a obter</param>
    /// <returns>Item da navegação</returns>
    NavigationViewItem? GetSelectedItem(Type pageType);
}
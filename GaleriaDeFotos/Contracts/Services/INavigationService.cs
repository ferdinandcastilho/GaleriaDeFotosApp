using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GaleriaDeFotos.Contracts.Services;

/// <summary>
///     Interface para o Serviço de Navegação para Paginas
/// </summary>
public interface INavigationService
{
    /// <summary>
    ///     Obtém se a view aceita que volte a página anterior
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    ///     Suporta a navegação, veja: <see cref="Frame" />
    /// </summary>
    Frame? Frame { get; set; }

    /// <summary>
    ///     Evento Disparado quando a navegação é efetuada
    /// </summary>
    event NavigatedEventHandler Navigated;

    /// <summary>
    ///     Navega para um página
    /// </summary>
    /// <param name="pageKey">Chave da página</param>
    /// <param name="parameter">Parâmetro para passar para a página para a qual está sendo navegada</param>
    /// <param name="clearNavigation">Limpa o Histórico de Navegação</param>
    /// <returns></returns>
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    /// <summary>
    ///     Volta para a página Anterior
    /// </summary>
    /// <returns>Retorna se a navegação pode ser feita, veja <see cref="CanGoBack" /></returns>
    bool GoBack();

    /// <summary>
    ///     Acessório para animação
    /// </summary>
    /// <param name="item">Para quem a animação vai ser voltada</param>
    void SetListDataItemForNextConnectedAnimation(object item);
}
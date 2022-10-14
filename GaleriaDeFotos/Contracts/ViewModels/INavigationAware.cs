namespace GaleriaDeFotos.Contracts.ViewModels;

/// <summary>
///     Interface para Páginas que estão cientes de Navegação
/// </summary>
public interface INavigationAware
{
    /// <summary>
    ///     Disparado quando uma página navega para esta
    /// </summary>
    /// <param name="parameter">Parametro enviado da Página anterior</param>
    void OnNavigatedTo(object parameter);

    /// <summary>
    ///     Disparado quando esta página navega para outra
    /// </summary>
    void OnNavigatedFrom();
}
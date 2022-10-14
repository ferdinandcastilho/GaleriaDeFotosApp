namespace GaleriaDeFotos.Contracts.Services;

/// <summary>
///     Interface do Serviço de páginas
/// </summary>
public interface IPageService
{
    /// <summary>
    ///     Obtém o Tipo de Página
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Type GetPageType(string key);
}
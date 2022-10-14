using System.Linq.Expressions;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.Contracts.Services;

/// <summary>
///     Serviço que controla o Banco de Dados de Imagens
/// </summary>
public interface IFotosDataService
{
    /// <summary>
    ///     Obtém as Fotos do Banco de Dados ou cadastra as fotos da pasta no Banco
    /// </summary>
    /// <param name="imagePath">Pasta para Carregar as Imagens</param>
    /// <returns>Fotos da Pasta Atual</returns>
    Task<IEnumerable<Foto>> GetPhotosAsync(string imagePath);

    /// <summary>
    ///     Executa uma condicional para obter uma Foto banco de imagens
    /// </summary>
    /// <param name="predicate">Condicional para obtenção da Foto</param>
    /// <returns>Foto que atende à condicional</returns>
    Task<IEnumerable<Foto>> SelectAsync(Expression<Func<FotoData, bool>> predicate);

    /// <summary>
    ///     Seta a condição de favorito da foto
    /// </summary>
    /// <param name="foto">Foto a ser setada/resetada como favorita</param>
    /// <param name="isFavorite">Condição de favorita</param>
    void SetFavorite(Foto foto, bool isFavorite);
}
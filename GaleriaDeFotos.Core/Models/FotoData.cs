namespace GaleriaDeFotos.Core.Models;

/// <summary>
///     Foto Data, para uso no Banco de Dados
/// </summary>
public class FotoData
{
    /// <summary>
    ///     Hash Único da Imagem
    /// </summary>
    public string ImageId { get; set; }

    /// <summary>
    ///     Caminho para o Arquivo de Imagem
    /// </summary>
    public string ImageUri { get; set; }

    /// <summary>
    ///     É Favorita?
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    ///     Pasta que contém a Imagem
    /// </summary>
    public string Folder { get; set; }
}
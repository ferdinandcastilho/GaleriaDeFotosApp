namespace GaleriaDeFotos.Core.Contracts.Services;

/// <summary>
///     Serviço de Arquivo
/// </summary>
public interface IFileService
{
    /// <summary>
    ///     Lê Arquivo
    /// </summary>
    /// <param name="folderPath">Caminho para o arquivo</param>
    /// <param name="fileName">Nome do Arquivo</param>
    /// <typeparam name="T">Tipo lido do Arquivo</typeparam>
    /// <returns>Tipo lido do Arquivo</returns>
    T Read<T>(string folderPath, string fileName);

    /// <summary>
    ///     Salva Arquivo
    /// </summary>
    /// <param name="folderPath">Caminho para o arquivo</param>
    /// <param name="fileName">Nome do Arquivo</param>
    /// <param name="content">Conteúdo a ser salvo</param>
    /// <typeparam name="T">Tipo lido do Arquivo</typeparam>
    void Save<T>(string folderPath, string fileName, T content);

    /// <summary>
    ///     Apaga Arquivo
    /// </summary>
    /// <param name="folderPath">Caminho para o arquivo</param>
    /// <param name="fileName">Nome do Arquivo</param>
    void Delete(string folderPath, string fileName);
}
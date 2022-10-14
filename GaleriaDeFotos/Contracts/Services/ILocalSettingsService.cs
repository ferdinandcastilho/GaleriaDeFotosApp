namespace GaleriaDeFotos.Contracts.Services;

/// <summary>
///     Interface para Configurações que são lidas do disco
/// </summary>
public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);
}
using GaleriaDeFotos.Contracts.Services;
using JetBrains.Annotations;

namespace GaleriaDeFotos.Contracts.Abstractions;

/// <summary>
///     Esta classe serve como base para uma configuração que funciona por meio de seletores
/// </summary>
/// <typeparam name="T">Tipo que será usado no Seletor</typeparam>
public abstract class SettingSelectorService<T> where T : struct, Enum
{
    private readonly ILocalSettingsService _localSettingsService;

    protected SettingSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    /// <summary>
    ///     Opção atualmente selecionada
    /// </summary>
    public T Setting { get; private set; }

    /// <summary>
    ///     Chave para a opção atualmente selecionada
    /// </summary>
    protected abstract string SettingKey { get; }

    /// <summary>
    ///     Seta a opção desejada
    /// </summary>
    /// <param name="value">Opção a ser setada</param>
    public async Task SetSetting(T value)
    {
        Setting = value;
        await SaveObjectInSettingsAsync();
    }

    /// <summary>
    ///     Inicializa esta configuração
    /// </summary>
    public async Task InitializeAsync() { Setting = await LoadObjectFromSettingsAsync(); }

    /// <summary>
    ///     Carrega opção das configurações
    /// </summary>
    /// <returns>Opção que foi carregada do Disco</returns>
    /// <exception cref="InvalidOperationException">A conversão foi mal sucedida</exception>
    private async Task<T> LoadObjectFromSettingsAsync()
    {
        var setting = await _localSettingsService.ReadSettingAsync<string>(SettingKey);
        if (Enum.TryParse(setting, out T cachedSetting)) return cachedSetting;
        if (typeof(T) == typeof(string))
            return (T)(Convert.ChangeType(setting, typeof(T)) ??
                       throw new InvalidOperationException());

        return default;
    }

    /// <summary>
    ///     Salva Opção no Disco
    /// </summary>
    [UsedImplicitly]
    protected async Task SaveObjectInSettingsAsync()
    {
        await _localSettingsService.SaveSettingAsync(SettingKey, Setting.ToString());
    }
}
using GaleriaDeFotos.Contracts.Services;
using JetBrains.Annotations;

namespace GaleriaDeFotos.Contracts.Abstractions;

public abstract class SettingSelectorService<T> where T : struct, Enum
{
    public T Setting { get; private set; }
    protected abstract string SettingKey { get; }

    private readonly ILocalSettingsService _localSettingsService;

    protected SettingSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task SetSetting(T value)
    {
        Setting = value;
        await SaveObjectInSettingsAsync();
    }

    public async Task InitializeAsync() { Setting = await LoadObjectFromSettingsAsync(); }

    private async Task<T> LoadObjectFromSettingsAsync()
    {
        var setting = await _localSettingsService.ReadSettingAsync<string>(SettingKey);
        if (Enum.TryParse(setting, out T cachedSetting)) return cachedSetting;
        if (typeof(T) == typeof(string))
        {
            return (T)(Convert.ChangeType(setting, typeof(T)) ??
                       throw new InvalidOperationException());
        }

        return default;
    }

    [UsedImplicitly]
    protected async Task SaveObjectInSettingsAsync()
    {
        await _localSettingsService.SaveSettingAsync(SettingKey, Setting.ToString());
    }
}
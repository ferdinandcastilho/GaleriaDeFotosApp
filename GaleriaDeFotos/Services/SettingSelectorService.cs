using GaleriaDeFotos.Contracts.Services;
using JetBrains.Annotations;
using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.Services;

public abstract class SettingSelectorService<T> where T : struct
{
    protected abstract string SettingsKey { get; }

    protected readonly ILocalSettingsService LocalSettingsService;

    protected SettingSelectorService(ILocalSettingsService localSettingsService)
    {
        LocalSettingsService = localSettingsService;
    }

    #region IThemeSelectorService Members

    [UsedImplicitly] public T Setting { get; set; }

    public async Task InitializeAsync()
    {
        Setting = await LoadObjectFromSettingsAsync();
        await Task.CompletedTask;
    }

    #endregion

    private async Task<T> LoadObjectFromSettingsAsync()
    {
        var setting = await LocalSettingsService.ReadSettingAsync<string>(SettingsKey);
        if (Enum.TryParse(setting, out T cacheTheme)) return cacheTheme;
        if (typeof(T) == typeof(string))
        {
            return (T)(Convert.ChangeType(setting, typeof(T)) ??
                       throw new InvalidOperationException());
        }

        return default;
    }

    [UsedImplicitly]
    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await LocalSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
using GaleriaDeFotos.Contracts.Abstractions;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.Settings;
using GaleriaDeFotos.Helpers;
using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.Services.Settings;

public class ThemeSelectorService : SettingSelectorService<ElementTheme>, IThemeSelectorService
{
    public ThemeSelectorService(ILocalSettingsService localSettingsService) : base(
        localSettingsService)
    {
    }

    protected override string SettingKey => nameof(ElementTheme);

    #region IThemeSelectorService Members

    public async Task SetThemeAsync(ElementTheme theme)
    {
        await SetSetting(theme);
        await SetRequestedThemeAsync();
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Setting;

            TitleBarHelper.UpdateTitleBar(Setting);
        }

        await Task.CompletedTask;
    }

    #endregion
}
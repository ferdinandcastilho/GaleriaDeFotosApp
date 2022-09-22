using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Helpers;
using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.Services;

public class ThemeSelectorService : SettingSelectorService<ElementTheme>, IThemeSelectorService
{
    public ThemeSelectorService(ILocalSettingsService localSettingsService) : base(
        localSettingsService)
    {
    }

    #region IThemeSelectorService Members

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    protected override string SettingsKey { get; } = "AppBackgroundRequestedTheme";

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    #endregion


    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await LocalSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
}
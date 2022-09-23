using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.Contracts.Settings;

public interface IThemeSelectorService
{
    ElementTheme Setting { get; }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
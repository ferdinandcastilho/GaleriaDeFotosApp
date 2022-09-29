using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Settings;
using GaleriaDeFotos.EnumTypes;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Services.Settings;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

namespace GaleriaDeFotos.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly LastFolderOptionSelectorService _lastFolderOptionSelectorService;
    [ObservableProperty] private ElementTheme _elementTheme;
    [ObservableProperty] private LastFolderOption _lastFolderOption;
    private string _versionDescription;

    public SettingsViewModel(IThemeSelectorService themeSelectorService,
        LastFolderOptionSelectorService lastFolderOptionSelectorService)
    {
        _lastFolderOptionSelectorService = lastFolderOptionSelectorService;
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Setting;
        _lastFolderOption = _lastFolderOptionSelectorService.Setting;
        _versionDescription = GetVersionDescription();
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    [RelayCommand]
    private async void SwitchLastFolderOption(LastFolderOption option)
    {
        if (LastFolderOption != option)
        {
            LastFolderOption = option;
            await _lastFolderOptionSelectorService.SetSetting(option);
        }

        await Task.CompletedTask;
    }

    [RelayCommand]
    private async void SwitchTheme(ElementTheme param)
    {
        if (ElementTheme != param)
        {
            ElementTheme = param;
            await _themeSelectorService.SetThemeAsync(param);
        }
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMsix)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build,
                packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return
            $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
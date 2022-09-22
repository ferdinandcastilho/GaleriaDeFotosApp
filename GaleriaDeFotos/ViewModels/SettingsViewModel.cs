using System.Reflection;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Models;
using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    [ObservableProperty] private ElementTheme _elementTheme;
    [ObservableProperty] private LastFolderBehavior _folderBehavior;
    private string _versionDescription;

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    [RelayCommand]
    private async void SwitchLastFolderBehavior(LastFolderBehavior behavior)
    {
        Console.WriteLine($"Behavior: {behavior}");
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
        } else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return
            $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
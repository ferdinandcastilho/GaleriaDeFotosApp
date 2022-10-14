using System.Reflection;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Settings;
using GaleriaDeFotos.EnumTypes;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Services.Settings;
using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.ViewModels;

/// <inheritdoc />
/// <summary>
///     ViewModel das Configurações
/// </summary>
public partial class SettingsViewModel : ObservableRecipient
{
    private readonly LastFolderOptionSelectorService _lastFolderOptionSelectorService;
    private readonly IThemeSelectorService _themeSelectorService;
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

    /// <summary>
    ///     Descrição da Versão
    /// </summary>
    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    /// <summary>
    ///     Muda a opção de Última Pasta
    /// </summary>
    /// <param name="option">Opção</param>
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

    /// <summary>
    ///     Troca Tema
    /// </summary>
    /// <param name="theme">Tema Escolhido</param>
    [RelayCommand]
    private async void SwitchTheme(ElementTheme theme)
    {
        if (ElementTheme != theme)
        {
            ElementTheme = theme;
            await _themeSelectorService.SetThemeAsync(theme);
        }
    }

    /// <summary>
    ///     Obtém Descrição da Versão
    /// </summary>
    /// <returns></returns>
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
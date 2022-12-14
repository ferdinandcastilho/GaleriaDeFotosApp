using Windows.Storage;
using GaleriaDeFotos.Activation;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.Settings;
using GaleriaDeFotos.Core.Services;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Services.Settings;
using GaleriaDeFotos.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Services;

/// <inheritdoc />
/// <summary>
///     Serviço de Ativação
/// </summary>
public class ActivationService : IActivationService
{
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly LastFolderOptionSelectorService _lastFolderOptionSelectorService;
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IThemeSelectorService themeSelectorService,
        LastFolderOptionSelectorService lastFolderOptionSelectorService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _lastFolderOptionSelectorService = lastFolderOptionSelectorService;
    }

    #region IActivationService Members

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    #endregion

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler =
            _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null) await activationHandler.HandleAsync(activationArgs);

        if (_defaultHandler.CanHandle(activationArgs))
            await _defaultHandler.HandleAsync(activationArgs);
    }

    /// <summary>
    ///     Executado na Inicialização
    /// </summary>
    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await _lastFolderOptionSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Executado no Início do Programa
    /// </summary>
    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();

        RuntimeConfigData.IsMsix = RuntimeHelper.IsMsix;
        RuntimeConfigData.ApplicationFolder = RuntimeHelper.IsMsix
            ? ApplicationData.Current.LocalFolder.Path
            : Environment.CurrentDirectory;

        await Task.CompletedTask;
    }
}
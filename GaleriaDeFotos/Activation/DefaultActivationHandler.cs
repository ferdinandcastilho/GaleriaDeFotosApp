using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml;

namespace GaleriaDeFotos.Activation;

/// <summary>
///     Classe Padrão da Ativação do Aplicativo
///     Veja: https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
/// </summary>
public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(MainViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
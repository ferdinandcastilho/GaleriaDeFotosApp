namespace GaleriaDeFotos.Activation;

/// <summary>
///     Interface da Ativação do Aplicativo
///     Veja: https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
/// </summary>
public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
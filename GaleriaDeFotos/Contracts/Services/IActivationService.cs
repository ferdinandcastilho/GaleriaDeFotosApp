namespace GaleriaDeFotos.Contracts.Services;

/// <summary>
///     Interface base para o serviço de Ativação do Aplicativo
///     Veja: https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
/// </summary>
public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
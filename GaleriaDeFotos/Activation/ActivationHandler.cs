namespace GaleriaDeFotos.Activation;

/// <summary>
///     Classe abstrata base da Ativação do Aplicativo
///     Veja: https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ActivationHandler<T> : IActivationHandler where T : class
{
    #region IActivationHandler Members

    public bool CanHandle(object args) { return args is T && CanHandleInternal((args as T)!); }

    public async Task HandleAsync(object args) { await HandleInternalAsync((args as T)!); }

    #endregion

    // Override this method to add the logic for whether to handle the activation.
    protected virtual bool CanHandleInternal(T args) { return true; }

    // Override this method to add the logic for your activation handler.
    protected abstract Task HandleInternalAsync(T args);
}
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.WinUI.UI.Animations;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GaleriaDeFotos.Services;

/// <inheritdoc />
/// <summary>
///     For more information on navigation between pages see
///     https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
/// </summary>
public class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private Frame? _frame;
    private object? _lastParameterUsed;

    public NavigationService(IPageService pageService) { _pageService = pageService; }

    #region INavigationService Members

    public event NavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = App.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }
        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = _frame.GetPageViewModel();
            _frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
                navigationAware.OnNavigatedFrom();

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);

        if (_frame != null && (_frame.Content?.GetType() != pageType ||
                               (parameter != null && !parameter.Equals(_lastParameterUsed))))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var navigated = _frame.Navigate(pageType, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                    navigationAware.OnNavigatedFrom();
            }

            return navigated;
        }

        return false;
    }

    public void SetListDataItemForNextConnectedAnimation(object item)
    {
        Frame.SetListDataItemForNextConnectedAnimation(item);
    }

    #endregion

    /// <summary>
    ///     Registra eventos do Frame
    /// </summary>
    private void RegisterFrameEvents()
    {
        if (_frame != null) _frame.Navigated += OnNavigated;
    }

    /// <summary>
    ///     Remove registro de eventos de Frame
    /// </summary>
    private void UnregisterFrameEvents()
    {
        if (_frame != null) _frame.Navigated -= OnNavigated;
    }

    /// <summary>
    ///     Executado ao navegar para a página
    /// </summary>
    /// <param name="sender">Objeto de origem do evento</param>
    /// <param name="e">Argumentos do Evento, veja <see cref="NavigationEventArgs" /></param>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is not Frame frame) return;
        var clearNavigation = (bool)frame.Tag;
        if (clearNavigation) frame.BackStack.Clear();

        if (frame.GetPageViewModel() is INavigationAware navigationAware)
            navigationAware.OnNavigatedTo(e.Parameter);

        Navigated?.Invoke(sender, e);
    }
}
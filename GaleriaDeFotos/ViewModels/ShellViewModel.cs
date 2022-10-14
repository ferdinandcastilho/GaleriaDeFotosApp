﻿using CommunityToolkit.Mvvm.ComponentModel;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Views;
using Microsoft.UI.Xaml.Navigation;

namespace GaleriaDeFotos.ViewModels;

/// <inheritdoc />
/// <summary>
///     Shell
/// </summary>
public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;
    private object? _selected;

    public ShellViewModel(INavigationService navigationService,
        INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        set => SetProperty(ref _isBackEnabled, value);
    }

    public object? Selected { get => _selected; set => SetProperty(ref _selected, value); }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null) Selected = selectedItem;
    }
}
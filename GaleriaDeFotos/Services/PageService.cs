﻿using CommunityToolkit.Mvvm.ComponentModel;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.ViewModels;
using GaleriaDeFotos.Views;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<FotosViewModel, FotosPage>();
        Configure<FotosDetailViewModel, FotosDetailPage>();
        Configure<FavoritasViewModel, FavoritasPage>();
        Configure<BlankViewModel, BlankPage>();
        Configure<SettingsViewModel, SettingsPage>();
    }

    #region IPageService Members

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
                throw new ArgumentException(
                    $"Page not found: {key}. Did you forget to call PageService.Configure?");
        }

        return pageType;
    }

    #endregion

    private void Configure<TVm, TV>() where TVm : ObservableObject where TV : Page
    {
        lock (_pages)
        {
            var key = typeof(TVm).FullName!;
            if (_pages.ContainsKey(key))
                throw new ArgumentException($"The key {key} is already configured in PageService");

            var type = typeof(TV);
            if (_pages.Any(p => p.Value == type))
                throw new ArgumentException(
                    $"This type is already configured with key {_pages.First(p => p.Value == type).Key}");

            _pages.Add(key, type);
        }
    }
}
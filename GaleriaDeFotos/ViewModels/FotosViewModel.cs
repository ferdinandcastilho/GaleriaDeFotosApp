﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using Microsoft.UI.Xaml.Controls;
using Windows.Security.Cryptography.Core;
using GaleriaDeFotos.Services;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private ObservableCollection<Foto> _source = new();
    [ObservableProperty] private Foto? _selectedFoto;
    [ObservableProperty] private bool _isLoading;

    public FotosViewModel(INavigationService navigationService, IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        var photos = await OpenLastOpenedFolder();
        Source = photos is not null ? new(photos) : Source;
    }

    public void OnNavigatedFrom()
    {

    }

    #endregion

    [RelayCommand]
    private void ItemClick(Foto? clickedItem)
    {
        if (clickedItem == null) return;
        _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
        _navigationService.NavigateTo(typeof(FotosFullViewModel).FullName!, clickedItem.ImageId);
    }

    [RelayCommand]
    private async Task SelectDirectory()
    {
        var folderPath = await FolderPicker();

        if (folderPath is not null)
        {
            IsLoading = true;

            var fotos = await _fotosDataService.GetPhotosAsync(folderPath);
            if (fotos.Any())
            {
                Source = new(fotos);

                IsLoading = false;
            }
        }
    }

    private static async Task<string?> FolderPicker()
    {
        FolderPicker folderPicker = new()
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            ViewMode = PickerViewMode.Thumbnail
        };

        var hwnd = App.MainWindow.GetWindowHandle();
        InitializeWithWindow.Initialize(folderPicker, hwnd);

        StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        return folder?.Path;
    }

    private async Task<IEnumerable<Foto>> OpenLastOpenedFolder()
    {
        var settings = App.GetService<ILocalSettingsService>();
        string folderToReadPhotos = await settings.ReadSettingAsync<string?>("LastFolder");

        if (folderToReadPhotos is not null) return await _fotosDataService.GetPhotosAsync(folderToReadPhotos);
        else return null;
    }
}
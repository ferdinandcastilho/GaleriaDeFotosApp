using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private readonly INavigationService _navigationService;
    public ObservableCollection<Foto> Source { get; } = new();

    [ObservableProperty] private Foto? _selectedFoto;
    [ObservableProperty] private bool _showFolderPicker = true;
    [ObservableProperty] private bool _showPhotos;
    [ObservableProperty] private bool _isLoading;

    public FotosViewModel(INavigationService navigationService, IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }


    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _fotosDataService.GetPhotosAsync();
        foreach (var item in data) Source.Add(item);
    }

    public void OnNavigatedFrom()
    {
        Source.Clear();
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
    private async Task PickDirectory()
    {
        FolderPicker folderPicker = new()
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            ViewMode = PickerViewMode.Thumbnail
        };

        var hwnd = App.MainWindow.GetWindowHandle();
        InitializeWithWindow.Initialize(folderPicker, hwnd);

        StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        var fotos = await _fotosDataService.GetPhotosAsync(folder?.Path);
        if (fotos?.Count() > 0)
        {
            IsLoading = true; 
            ShowFolderPicker = false; 
            ShowPhotos = true; 

            foreach (var foto in fotos) { Source.Add(foto); }

            await Task.Delay(3000);
            IsLoading = false;
        } // Substituir ObservableProperties por Converter
    }
}
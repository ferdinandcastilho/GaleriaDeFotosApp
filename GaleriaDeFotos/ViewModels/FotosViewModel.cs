using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using Windows.Storage.Pickers;
using GaleriaDeFotos.EnumTypes;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Services;
using GaleriaDeFotos.Services.Settings;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private readonly INavigationService _navigationService;
    private readonly LastFolderOptionSelectorService _lastFolderOptionSelectorService;
    private bool _folderAlreadyPicked;

    [ObservableProperty] private ObservableCollection<Foto> _source = new();
    [ObservableProperty] private Foto? _selectedFoto;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _needToPickFolder;

    public Visibility PickFolderVisibility
    {
        get => Source.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }


    public Visibility NotPickFolderVisibility
    {
        get =>
            PickFolderVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    public FotosViewModel(INavigationService navigationService, IFotosDataService fotosDataService,
        LastFolderOptionSelectorService lastFolderOptionSelectorService)
    {
        _lastFolderOptionSelectorService = lastFolderOptionSelectorService;
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
        Source.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(BottomBar));
            OnPropertyChanged(nameof(PickFolderVisibility));
            OnPropertyChanged(nameof(NotPickFolderVisibility));
        };
    }

    public string BottomBar => $"{Source.Count} {"FotosPage_Items".GetLocalized()}";

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        var option = _lastFolderOptionSelectorService.Setting;
        NeedToPickFolder = option == LastFolderOption.AlwaysPick;
        if (!NeedToPickFolder || _folderAlreadyPicked)
        {
            await OpenLastOpenedFolder();
        }
    }

    public void OnNavigatedFrom() { }

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
        FolderPicker folderPicker = new()
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            ViewMode = PickerViewMode.Thumbnail
        };

        var windowHandle = App.MainWindow.GetWindowHandle();
        InitializeWithWindow.Initialize(folderPicker, windowHandle);

        var folder = await folderPicker.PickSingleFolderAsync();

        if (!string.IsNullOrEmpty(folder?.Path))
        {
            _folderAlreadyPicked = true;
            await ReadPhotosFromFolder(folder.Path);
        }
    }

    private async Task OpenLastOpenedFolder()
    {
        var settings = App.GetService<ILocalSettingsService>();
        var folderToReadPhotos =
            await settings.ReadSettingAsync<string?>(LocalSettingsService.LastFolderKey);

        await ReadPhotosFromFolder(folderToReadPhotos);
    }

    private async Task ReadPhotosFromFolder(string? path)
    {
        IsLoading = true;
        var dbPhotos = (await _fotosDataService.GetPhotosAsync(path)).ToList();
        Source.Clear();
        foreach (var photo in dbPhotos)
        {
            Source.Add(photo);
        }

        await Task.Delay(1000);
        IsLoading = false;
    }
}
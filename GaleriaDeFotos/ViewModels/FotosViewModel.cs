using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.EnumTypes;
using GaleriaDeFotos.Services;
using GaleriaDeFotos.Services.Settings;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosViewModel : BaseFotosViewModel
{
    private readonly IFotosDataService _fotosDataService;
    private readonly LastFolderOptionSelectorService _lastFolderOptionSelectorService;

    private string? _currentFolder = string.Empty;
    private bool _folderAlreadyPicked;

    [ObservableProperty] private bool _needToPickFolder;
    [ObservableProperty] private Foto? _selectedFoto;

    public FotosViewModel(INavigationService navigationService, IFotosDataService fotosDataService,
        LastFolderOptionSelectorService lastFolderOptionSelectorService) : base(navigationService)
    {
        _lastFolderOptionSelectorService = lastFolderOptionSelectorService;

        _fotosDataService = fotosDataService;
        Source.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(BottomBar));
            OnPropertyChanged(nameof(PickFolderVisibility));
            OnPropertyChanged(nameof(NotPickFolderVisibility));
        };
    }

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

    protected override async void OnNavigatedToChild(object parameter)
    {
        var option = _lastFolderOptionSelectorService.Setting;
        NeedToPickFolder = option == LastFolderOption.AlwaysPick;
        if (!NeedToPickFolder || _folderAlreadyPicked) await OpenLastOpenedFolder();
    }

    protected override void OnNavigatedFromChild() { }

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

        _currentFolder = folder?.Path;
    }

    protected override async Task LoadPhotos() { await ReadPhotosFromFolder(_currentFolder); }

    private async Task OpenLastOpenedFolder()
    {
        var settings = App.GetService<ILocalSettingsService>();
        var folderToReadPhotos =
            await settings.ReadSettingAsync<string?>(LocalSettingsService.LastFolderKey);
        _currentFolder = folderToReadPhotos;
        await ReadPhotosFromFolder(folderToReadPhotos);
    }

    private async Task ReadPhotosFromFolder(string? path)
    {
        IsLoading = true;
        if (path == null) return;
        var dbPhotos = (await _fotosDataService.GetPhotosAsync(path)).ToList();
        Source.Clear();
        foreach (var photo in dbPhotos) Source.Add(photo);

        await Task.Delay(1000);
        IsLoading = false;
    }
}
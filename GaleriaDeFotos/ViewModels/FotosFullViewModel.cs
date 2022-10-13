using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Models;
using GaleriaDeFotos.Templates;
using ImageProcessor;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosFullViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private BaseFotosViewModel? _lastViewModel;
    private readonly INavigationService _navigationService;
    [ObservableProperty] private bool _isFavorite;
    [ObservableProperty] private bool _isShowingDetails;
    [ObservableProperty] private int _imageWidth;
    [ObservableProperty] private int _imageHeight;
    [ObservableProperty] private double _imageAspectRatio;
    [ObservableProperty] private Foto? _item;
    [ObservableProperty] private Foto? _selectedFoto;

    [ObservableProperty] private ImageFactory _imageFactory = new();
    //Propriedades

    public FotosFullViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    public string FotoSizeString
    {
        get
        {
            if (Item == null)
            {
                return string.Empty;
            }

            var size = _imageFactory.Image.Size;
            return $"{size.Width} x {size.Height}";
        }
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    public string FileSizeString
    {
        get
        {
            if (Item == null)
            {
                return string.Empty;
            }

            var fi = new FileInfo(Item.ImageUri.LocalPath);
            var mb = fi.Length / 1000000;
            return mb > 1.0
                ? $"{fi.Length / 1000000.0:0.00} MBs"
                : $"{fi.Length / 1000.0:0.00} kBs";
        }
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    public string FileCreatedString
    {
        get
        {
            if (Item == null)
            {
                return string.Empty;
            }

            var fi = new FileInfo(Item.ImageUri.LocalPath);
            return $"{fi.CreationTime:HH:mm:ss dd/MM/yyyy}";
        }
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is not FotoParameters param) return;
        var found = _fotosDataService.Select(fotoData => fotoData.ImageId == param.ImageId);
        Item = found.FirstOrDefault();
        if (Item == null) return;
        await using var imageStream = File.OpenRead(Item.ImageUri.LocalPath);
        _imageFactory.Load(imageStream).AutoRotate();
        _imageWidth = _imageFactory.Image.Width;
        _imageHeight = _imageFactory.Image.Height;
        _imageAspectRatio = (double)_imageHeight / _imageWidth;
        IsFavorite = Item.IsFavorite;


        if (param.BaseFotosViewModel is not { } fotosViewModel) return;
        _lastViewModel = fotosViewModel;
        await Task.CompletedTask;
    }

    public void OnNavigatedFrom() { }

    #endregion

    public void UpdateResizeHeight() { ImageHeight = (int)(_imageWidth * ImageAspectRatio); }

    private async void ResizeImage()
    {
        if (Item == null) return;
        var filePath = Item.ImageUri.LocalPath;
        var photoBytes = await File.ReadAllBytesAsync(filePath);
        using var inStream = new MemoryStream(photoBytes);
        using var outStream = new MemoryStream();
        using var imageFactory = new ImageFactory(preserveExifData: true);
        var extension = Path.GetExtension(filePath);
        var newFileNoExtension = filePath.Remove(filePath.IndexOf('.'));
        var newFilePath = $"{newFileNoExtension + "_edited" + extension}";
        imageFactory.Load(inStream);
        var size = new Size(_imageWidth, _imageHeight);
        imageFactory.Resize(size).Save(newFilePath);
        var fileName = Path.GetFileName(newFilePath);
        if (MainWindow.Instance == null) return;
        await MainWindow.Instance.ShowMessageDialogAsync(
            $"Image Redimensionada salva como \n{fileName}", "Imagem Redimensionada");
    }

    [RelayCommand]
    private void ToggleDetails()
    {
        if (Item == null) return;
        IsShowingDetails = !IsShowingDetails;
    }

    [RelayCommand]
    private void SetFavorite()
    {
        _fotosDataService.SetFavorite(Item, true);
        IsFavorite = true;
    }


    [RelayCommand]
    private void UnSetFavorite()
    {
        _fotosDataService.SetFavorite(Item, false);
        IsFavorite = false;
    }

    [RelayCommand]
    private async void OpenResizeWindow()
    {
        if (MainWindow.Instance == null) return;
        var messageDialog = new ResizeBox
        {
            Title = "Redimensionamento de Imagem",
            XamlRoot = MainWindow.Instance.Content.XamlRoot,
            PrimaryButtonText = "Ok",
            CloseButtonText = "Cancelar"
        };
        var result = await messageDialog.ShowAsync();
        if (result == ContentDialogResult.Primary) ResizeImage();
    }

    [RelayCommand]
    private async void DeleteImage()
    {
        if (Item == null) return;
        if (Item.IsFavorite)
        {
            UnSetFavorite();
        }

        File.Delete(Item.ImageUri.LocalPath);
        if (_lastViewModel == null) return;
        await Task.Delay(1000);
        _navigationService.NavigateTo(_lastViewModel.GetType().FullName!, true);
    }
}
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
    [ObservableProperty] private Foto _item = new();
    [ObservableProperty] private Foto? _selectedFoto;

    [ObservableProperty] private ImageFactory _imageFactory = new(preserveExifData: true);
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
            var fi = new FileInfo(Item.ImageUri.LocalPath);
            return $"{fi.CreationTime:HH:mm:ss dd/MM/yyyy}";
        }
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    /// <summary>
    /// Carrega a Imagem para o ViewModel
    /// </summary>
    /// <param name="image"></param>
    /// <returns>True se Carregado com Sucesso, False se não</returns>
    private async Task<bool> LoadImageAsync(Foto image)
    {
        var photoBytes = await File.ReadAllBytesAsync(image.ImageUri.LocalPath);
        using var imageStream = new MemoryStream(photoBytes);
        _imageFactory.Load(imageStream).AutoRotate();
        _imageWidth = _imageFactory.Image.Width;
        _imageHeight = _imageFactory.Image.Height;
        _imageAspectRatio = (double)_imageHeight / _imageWidth;
        var dumb = new Foto();
        Item = dumb;
        Item = image;
        return true;
    }

    /// <summary>
    /// Carrega a Imagem para o ViewModel
    /// </summary>
    /// <param name="imageId">Id da Imagem no DB</param>
    /// <returns>True se Carregado com Sucesso, False se não</returns>
    private async Task<bool> LoadImageAsync(string imageId)
    {
        var found = await _fotosDataService.SelectAsync(fotoData => fotoData.ImageId == imageId);
        var item = found.FirstOrDefault();
        if (item == null) return false;
        return await LoadImageAsync(item);
    }

    /// <summary>
    /// Carrega a Imagem para o ViewModel
    /// </summary>
    /// <param name="imageUri">Endereço da Imagem</param>
    /// <returns>True se Carregado com Sucesso, False se não</returns>
    private async Task<bool> LoadImageAsync(Uri imageUri)
    {
        var found =
            await _fotosDataService.SelectAsync(fotoData =>
                fotoData.ImageUri == imageUri.LocalPath);
        var item = found.FirstOrDefault();
        if (item == null) return false;
        return await LoadImageAsync(item);
    }

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is not FotoParameters param) return;
        if (!await LoadImageAsync(param.ImageId)) return;
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
        var size = new Size(_imageWidth, _imageHeight);
        _imageFactory.Resize(size);
        await SaveEditedAsync();
    }

    private async Task SaveEditedAsync()
    {
        using var outStream = new MemoryStream();
        var filePath = Item.ImageUri.LocalPath;
        var extension = Path.GetExtension(filePath);
        var newFileNoExtension = filePath.Remove(filePath.IndexOf('.'));
        var newFilePath = newFileNoExtension.Contains("_edited")
            ? filePath
            : $"{newFileNoExtension + "_edited" + extension}";
        _imageFactory.Save(newFilePath);
        var fileName = Path.GetFileName(newFilePath);
        if (MainWindow.Instance == null) return;

        var result = await LoadImageAsync(new Uri(newFilePath));
        string content;
        string title;
        if (result)
        {
            content = $"Image Editada salva como \n{fileName}";
            title = "Edição Completa";
        } else
        {
            content = "Erro Editando Imagem";
            title = "Erro";
        }

        await MainWindow.Instance.ShowMessageDialogAsync(content, title);
    }

    [RelayCommand]
    private void ToggleDetails() { IsShowingDetails = !IsShowingDetails; }

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
        if (Item.IsFavorite)
        {
            UnSetFavorite();
        }

        File.Delete(Item.ImageUri.LocalPath);
        if (_lastViewModel == null) return;
        await Task.Delay(1000);
        _navigationService.NavigateTo(_lastViewModel.GetType().FullName!, true);
    }

    [RelayCommand]
    private async void RotateImage()
    {
        _imageFactory.Rotate(90);
        await SaveEditedAsync();
        await Task.CompletedTask;
    }
}
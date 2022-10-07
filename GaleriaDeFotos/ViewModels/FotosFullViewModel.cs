using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using ImageProcessor;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosFullViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    [ObservableProperty] private bool _isFavorite;

    [ObservableProperty] private bool _isShowingDetails;
    [ObservableProperty] private Foto? _item;

    [ObservableProperty] private Foto? _selectedFoto;
    //Propriedades

    public FotosFullViewModel(IFotosDataService fotosDataService)
    {
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

            using var imageStream = File.OpenRead(Item.ImageUri.AbsolutePath);
            using var imageFactory = new ImageFactory();
            imageFactory.Load(imageStream).AutoRotate(); //takes care of ex-if
            var height = imageFactory.Image.Height;
            var width = imageFactory.Image.Width;

            return $"{width} x {height}";
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

            var fi = new FileInfo(Item.ImageUri.AbsolutePath);
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

            var fi = new FileInfo(Item.ImageUri.AbsolutePath);
            return $"{fi.CreationTime:HH:mm:ss dd/MM/yyyy}";
        }
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string imageId)
        {
            Item = _fotosDataService.Select(fotoData => fotoData.ImageId == imageId).First();
            IsFavorite = Item.IsFavorite;
        }

        await Task.CompletedTask;
    }

    public void OnNavigatedFrom() { }

    #endregion

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
}
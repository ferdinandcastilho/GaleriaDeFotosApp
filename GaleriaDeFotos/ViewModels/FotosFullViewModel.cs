using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosFullViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    [ObservableProperty] private string _fotoHeight = string.Empty;

    //Propriedades da Foto
    [ObservableProperty] private string _fotoWidth = string.Empty;
    [ObservableProperty] private bool _isFavorite;

    [ObservableProperty] private bool _isShowingDetails;
    [ObservableProperty] private Foto? _item;
    [ObservableProperty] private Foto? _selectedFoto;
    [ObservableProperty] private string _sizeString = string.Empty;

    public FotosFullViewModel(IFotosDataService fotosDataService)
    {
        _fotosDataService = fotosDataService;
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
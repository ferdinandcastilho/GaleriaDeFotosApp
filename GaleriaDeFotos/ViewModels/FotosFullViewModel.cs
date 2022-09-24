using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosFullViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private readonly INavigationService _navigationService;
    [ObservableProperty] private Foto? _item;
    [ObservableProperty] private Foto? _selectedFoto;

    public FotosFullViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string imageId)
        {
            Item = _fotosDataService.Select(fotoData => fotoData.ImageId == imageId).First();
        }

        await Task.CompletedTask;
    }

    public void OnNavigatedFrom() { }

    #endregion

    [RelayCommand]
    private void GetDetails()
    {
        if (Item == null) return;
        _navigationService.SetListDataItemForNextConnectedAnimation(Item);
        _navigationService.NavigateTo(typeof(FotosDetailViewModel).FullName!, Item.ImageId);
    }

    [RelayCommand]
    private void SetFavorite() { _fotosDataService.SetFavorite(Item, true); }


    [RelayCommand]
    private void UnSetFavorite() { _fotosDataService.SetFavorite(Item, false); }
}
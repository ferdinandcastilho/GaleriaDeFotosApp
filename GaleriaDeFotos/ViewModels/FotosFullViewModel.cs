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
    [ObservableProperty] private Foto? _item;
    private readonly INavigationService _navigationService;
    [ObservableProperty] private Foto? _selectedFoto;

    public FotosFullViewModel(INavigationService navigationService, IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    [RelayCommand]
    private void DetailClick()
    {
        if (Item == null) return;
        _navigationService.SetListDataItemForNextConnectedAnimation(Item);
        _navigationService.NavigateTo(typeof(FotosDetailViewModel).FullName!, Item.ImageId);
    }
    
    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long imageId)
        {
            var data = await _fotosDataService.GetPhotos();
            Item = data.First(i => i.ImageId == imageId);
        }
    }

    public void OnNavigatedFrom() { }

    #endregion
}
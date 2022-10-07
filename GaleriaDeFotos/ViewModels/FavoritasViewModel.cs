using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public partial class FavoritasViewModel : BaseFotosViewModel, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private readonly INavigationService _navigationService;

    public FavoritasViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }


    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        GetPhotos();

        await Task.CompletedTask;
    }

    private void GetPhotos()
    {
        IsLoading = true;
        var favorites = _fotosDataService.Select(data => data.IsFavorite);
        Source.Clear();
        foreach (var favorite in favorites) Source.Add(favorite);
        IsLoading = false;
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
    private void Refresh() { GetPhotos(); }
}
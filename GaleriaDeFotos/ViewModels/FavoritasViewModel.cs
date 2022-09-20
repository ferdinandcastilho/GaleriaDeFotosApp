using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public partial class FavoritasViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IFotosDataService _fotosDataService;

    public FavoritasViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    [RelayCommand]
    void ItemClick(Foto clickedItem)
    {
        _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
        _navigationService.NavigateTo(typeof(FotosDetailViewModel).FullName!, clickedItem.ImageId);
    }

    public ObservableCollection<Foto> Source { get; } = new();

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _fotosDataService.GetPhotosAsync();
        foreach (var item in data) Source.Add(item);
    }

    public void OnNavigatedFrom() { }

    #endregion
}
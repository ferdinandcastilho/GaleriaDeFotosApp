using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Helpers;

namespace GaleriaDeFotos.ViewModels;

public partial class FavoritasViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private readonly INavigationService _navigationService;

    public FavoritasViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    public string BottomBar
    {
        get => $"{Source.Count} {"FotosPage_Items".GetLocalized()}";
        // ReSharper disable once ValueParameterNotUsed
        set
        {
        }
    }

    public ObservableCollection<Foto> Source { get; } = new();

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        var favorites = _fotosDataService.Select(data => data.IsFavorite);
        Source.Clear();
        foreach (var favorite in favorites) Source.Add(favorite);

        await Task.CompletedTask;
    }

    public void OnNavigatedFrom()
    {
    }

    #endregion

    [RelayCommand]
    private void ItemClick(Foto? clickedItem)
    {
        if (clickedItem == null) return;
        _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
        _navigationService.NavigateTo(typeof(FotosFullViewModel).FullName!, clickedItem.ImageId);
    }
}
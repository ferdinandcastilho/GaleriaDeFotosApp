using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IFotosDataService _fotosDataService;
    [ObservableProperty] private Foto? _selectedFoto;

    public FotosViewModel(INavigationService navigationService, IFotosDataService fotosDataService)
    {
        _navigationService = navigationService;
        _fotosDataService = fotosDataService;
    }

    [RelayCommand]
    private void ItemClick(Foto? clickedItem)
    {
        if (clickedItem == null) return;
        _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
        _navigationService.NavigateTo(typeof(FotosFullViewModel).FullName!, clickedItem.ImageId);
    }

    public ObservableCollection<Foto> Source { get; } = new();

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _fotosDataService.GetPhotos();
        foreach (var item in data) Source.Add(item);
    }

    public void OnNavigatedFrom() { }

    #endregion
}
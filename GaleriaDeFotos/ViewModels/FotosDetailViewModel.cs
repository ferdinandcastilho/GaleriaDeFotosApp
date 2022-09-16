using CommunityToolkit.Mvvm.ComponentModel;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public class FotosDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    private Foto? _item;

    public FotosDetailViewModel(IFotosDataService fotosDataService)
    {
        _fotosDataService = fotosDataService;
    }

    public Foto? Item { get => _item; set => SetProperty(ref _item, value); }

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
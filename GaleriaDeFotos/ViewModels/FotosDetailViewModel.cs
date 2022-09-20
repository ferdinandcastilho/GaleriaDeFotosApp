using CommunityToolkit.Mvvm.ComponentModel;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.ViewModels;

public partial class FotosDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly IFotosDataService _fotosDataService;
    [ObservableProperty] private Foto? _item;
    public FotosDetailViewModel(IFotosDataService fotosDataService)
    {
        _fotosDataService = fotosDataService;
    }

    #region INavigationAware Members

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string imageId)
        {
            var data = await _fotosDataService.GetPhotos();
            Item = data.First(i => i.ImageId == imageId);
        }
    }

    public void OnNavigatedFrom() { }

    #endregion
}
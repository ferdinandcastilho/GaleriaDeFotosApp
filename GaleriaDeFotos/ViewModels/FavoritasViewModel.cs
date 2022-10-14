using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Core.Contracts.Services;

namespace GaleriaDeFotos.ViewModels;

public class FavoritasViewModel : BaseFotosViewModel
{
    private readonly IFotosDataService _fotosDataService;

    public FavoritasViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService) : base(navigationService)
    {
        _fotosDataService = fotosDataService;
    }

    #region INavigationAware Members

    protected override async void OnNavigatedToChild(object parameter)
    {
        await LoadPhotos();

        await Task.CompletedTask;
    }

    protected override void OnNavigatedFromChild() { }

    #endregion

    protected override async Task LoadPhotos()
    {
        Source.Clear();
        var favorites = await _fotosDataService.SelectAsync(data => data.IsFavorite);

        foreach (var favorite in favorites) Source.Add(favorite);
        await Task.CompletedTask;
    }
}
using GaleriaDeFotos.Contracts.Services;

namespace GaleriaDeFotos.ViewModels;

public class FavoritasViewModel : BaseFotosViewModel
{
    private readonly IFotosDataService _fotosDataService;

    public FavoritasViewModel(INavigationService navigationService,
        IFotosDataService fotosDataService) : base(navigationService)
    {
        _fotosDataService = fotosDataService;
    }

    protected override async void OnNavigatedToChild(object parameter)
    {
        await LoadPhotos();

        await Task.CompletedTask;
    }

    protected override void OnNavigatedFromChild() { }

    protected override async Task LoadPhotos()
    {
        Source.Clear();
        var favorites = await _fotosDataService.SelectAsync(data => data.IsFavorite);

        foreach (var favorite in favorites) Source.Add(favorite);
        await Task.CompletedTask;
    }
}
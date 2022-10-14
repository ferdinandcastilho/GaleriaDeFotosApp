using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Models;

namespace GaleriaDeFotos.ViewModels;

public abstract partial class BaseFotosViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;

    public BaseFotosViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    [ObservableProperty] protected bool _isLoading;
    [ObservableProperty] private ObservableCollection<Foto> _source = new();
    protected abstract Task LoadPhotos();

    public string BottomBar
    {
        get => $"{Source.Count} {"FotosPage_Items".GetLocalized()}";
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    [RelayCommand]
    protected void Refresh()
    {
        IsLoading = true;
        LoadPhotos();
        IsLoading = false;
    }

    [RelayCommand]
    private void ItemClick(Foto? clickedItem)
    {
        if (clickedItem == null) return;
        var param = new FotoParameters(clickedItem.ImageId, this);
        _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
        _navigationService.NavigateTo(typeof(FotosFullViewModel).FullName!, param);
    }

    public void OnNavigatedTo(object parameter)
    {
        OnNavigatedToChild(parameter);
    }

    public void OnNavigatedFrom() { OnNavigatedFromChild(); }

    protected abstract void OnNavigatedToChild(object parameter);

    protected abstract void OnNavigatedFromChild();
}
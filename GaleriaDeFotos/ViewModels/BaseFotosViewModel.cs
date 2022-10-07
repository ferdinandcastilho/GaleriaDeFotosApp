using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Helpers;

namespace GaleriaDeFotos.ViewModels;

public abstract partial class BaseFotosViewModel : ObservableRecipient
{
    // ReSharper disable once InconsistentNaming
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
    private void Refresh()
    {
        IsLoading = true;
        LoadPhotos();
        IsLoading = false;
    }
}
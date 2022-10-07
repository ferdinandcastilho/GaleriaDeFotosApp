using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Helpers;

namespace GaleriaDeFotos.ViewModels;

public partial class BaseFotosViewModel : ObservableRecipient
{
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] protected bool _isLoading;
    [ObservableProperty] private ObservableCollection<Foto> _source = new();

    public string BottomBar
    {
        get => $"{Source.Count} {"FotosPage_Items".GetLocalized()}";
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }
    
}
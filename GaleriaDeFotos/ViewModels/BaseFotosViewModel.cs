using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.ViewModels;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Models;

namespace GaleriaDeFotos.ViewModels;

/// <inheritdoc cref="GaleriaDeFotos.Contracts.ViewModels.INavigationAware" />
/// <summary>
///     View Model Base Para páginas que manipulam fotos
/// </summary>
public abstract partial class BaseFotosViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;

    public BaseFotosViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    } // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    /// <summary>
    ///     Identifica se a tela está carregando
    /// </summary>
    [ObservableProperty] protected bool _isLoading;

    /// <summary>
    ///     Fonte das fotos
    /// </summary>
    [ObservableProperty] private ObservableCollection<Foto> _source = new();

    /// <summary>
    ///     Carrega as Fotos
    /// </summary>
    /// <returns></returns>
    protected abstract Task LoadPhotos();

    /// <summary>
    ///     Barra Inferior
    /// </summary>
    public string BottomBar
    {
        get => $"{Source.Count} {"FotosPage_Items".GetLocalized()}";
        // ReSharper disable once ValueParameterNotUsed
        set { }
    }

    /// <summary>
    ///     Comando que atualiza as modificações das fotos
    /// </summary>
    [RelayCommand]
    protected void Refresh()
    {
        IsLoading = true;
        LoadPhotos();
        IsLoading = false;
    }

    /// <summary>
    ///     Executado ao clicar em uma foto
    /// </summary>
    /// <param name="clickedItem">Foto Clicada</param>
    [RelayCommand]
    private void ItemClick(Foto? clickedItem)
    {
        if (clickedItem == null) return;
        var param = new FotoParameters(clickedItem.ImageId, this);
        _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
        _navigationService.NavigateTo(typeof(FotosFullViewModel).FullName!, param);
    }

    public void OnNavigatedTo(object parameter) { OnNavigatedToChild(parameter); }

    public void OnNavigatedFrom() { OnNavigatedFromChild(); }

    /// <summary>
    ///     Disparado ao navegar para esta página <see cref="OnNavigatedTo" />
    /// </summary>
    /// <param name="parameter">
    ///     <see cref="OnNavigatedTo" />
    /// </param>
    protected abstract void OnNavigatedToChild(object parameter);

    /// <summary>
    ///     Disparado ao navegar desta página <see cref="OnNavigatedFrom" />
    /// </summary>
    protected abstract void OnNavigatedFromChild();
}
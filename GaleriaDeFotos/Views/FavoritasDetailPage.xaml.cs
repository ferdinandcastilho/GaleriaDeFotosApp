using CommunityToolkit.WinUI.UI.Animations;

using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GaleriaDeFotos.Views;

public sealed partial class FavoritasDetailPage : Page
{
    public FavoritasDetailViewModel ViewModel
    {
        get;
    }

    public FavoritasDetailPage()
    {
        ViewModel = App.GetService<FavoritasDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}

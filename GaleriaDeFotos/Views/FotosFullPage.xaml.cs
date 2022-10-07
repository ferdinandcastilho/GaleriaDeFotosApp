using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosFullPage
{
    public FotosFullPage()
    {
        ViewModel = App.GetService<FotosFullViewModel>();
        InitializeComponent();
    }

    public FotosFullViewModel ViewModel { get; }

    private void ShowHideTeachingTip(object sender, RoutedEventArgs e)
    {
        string teachingTipTitle;
        string teachingTipSubtitle;

        var appBarButton = e.OriginalSource as AppBarButton;

        if (appBarButton!.Label == "Favoritar")
        {
            teachingTipTitle = "FotosFullPage_TeachingTip_FavoritedTitle".GetLocalized();
            teachingTipSubtitle = "FotosFullPage_TeachingTip_FavoritedSubtitle".GetLocalized();
        } else
        {
            teachingTipTitle = "FotosFullPage_TeachingTip_UnfavoritedTitle".GetLocalized();
            teachingTipSubtitle = "FotosFullPage_TeachingTip_UnfavoritedSubtitle".GetLocalized();
        }

        FavoriteTeachingTip.Title = teachingTipTitle;
        FavoriteTeachingTip.Subtitle = teachingTipSubtitle;

        FavoriteTeachingTip.Target = e.OriginalSource as AppBarButton;
        FavoriteTeachingTip.IsOpen = true;
    }
}
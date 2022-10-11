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
        ToolTipService.SetToolTip(DetailsButton,
            "FotosFullPage_CommandBar_Details_ToolTip".GetLocalized());
        ToolTipService.SetToolTip(FavoriteButton,
            "FotosFullPage_CommandBar_Favorite_ToolTip".GetLocalized());
        ToolTipService.SetToolTip(UnFavoriteButton,
            "FotosFullPage_CommandBar_UnFavorite_ToolTip".GetLocalized());
        ToolTipService.SetToolTip(ResizeButton,
            "FotosFullPage_CommandBar_Resize_ToolTip".GetLocalized());
        ToolTipService.SetToolTip(DeleteButton,
            "FotosFullPage_CommandBar_Delete_ToolTip".GetLocalized());
        ToolTipService.SetToolTip(RotateButton,
            "FotosFullPage_CommandBar_Rotate_ToolTip".GetLocalized());
    }

    public FotosFullViewModel ViewModel { get; }

    private void ShowHideTeachingTip(object sender, RoutedEventArgs e)
    {
        var teachingTipTitle = string.Empty;
        var teachingTipSubtitle = string.Empty;

        var appBarButton = e.OriginalSource as AppBarButton;
        if (appBarButton == null) return;
        if (appBarButton.Label == "FotosFullPage_CommandBar_Favorite_Label".GetLocalized())
        {
            teachingTipTitle = "FotosFullPage_TeachingTip_UnFavoriteTitle".GetLocalized();
            teachingTipSubtitle = "FotosFullPage_TeachingTip_UnFavoriteSubtitle".GetLocalized();
        } else if (appBarButton.Label == "FotosFullPage_CommandBar_UnFavorite_Label".GetLocalized())
        {
            teachingTipTitle = "FotosFullPage_TeachingTip_FavoriteTitle".GetLocalized();
            teachingTipSubtitle = "FotosFullPage_TeachingTip_FavoriteSubtitle".GetLocalized();
        } else if (appBarButton.Label == "FotosFullPage_CommandBar_Delete_Label".GetLocalized())
        {
            teachingTipTitle = "FotosFullPage_TeachingTip_DeleteTitle".GetLocalized();
            teachingTipSubtitle = "FotosFullPage_TeachingTip_DeleteSubtitle".GetLocalized();
        } else if (appBarButton.Label == "FotosFullPage_CommandBar_Rotate_Label".GetLocalized())
        {
            teachingTipTitle = "FotosFullPage_TeachingTip_DeleteTitle".GetLocalized();
            teachingTipSubtitle = "FotosFullPage_TeachingTip_DeleteSubtitle".GetLocalized();
        }

        FavoriteTeachingTip.Title = teachingTipTitle;
        FavoriteTeachingTip.Subtitle = teachingTipSubtitle;

        FavoriteTeachingTip.Target = e.OriginalSource as AppBarButton;
        FavoriteTeachingTip.IsOpen = true;
    }
}
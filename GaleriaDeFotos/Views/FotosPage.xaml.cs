using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosPage
{
    public FotosPage()
    {
        ViewModel = App.GetService<FotosViewModel>();
        InitializeComponent();
    }

    public FotosViewModel ViewModel { get; }
}
using GaleriaDeFotos.ViewModels;
using Microsoft.UI.Xaml.Input;

namespace GaleriaDeFotos.Views;

public sealed partial class FotosPage
{
    public FotosPage()
    {
        ViewModel = App.GetService<FotosViewModel>();
        InitializeComponent();
    }

    public FotosViewModel ViewModel { get; }

    private async void SelectFolderOnTapped(object sender, TappedRoutedEventArgs e)
    {
        if (ViewModel.SelectDirectoryCommand.CanExecute(null))
        {
            await ViewModel.SelectDirectoryCommand.ExecuteAsync(null);
        }
    }
}
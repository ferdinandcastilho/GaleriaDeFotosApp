using CommunityToolkit.Mvvm.ComponentModel;

namespace GaleriaDeFotos.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public partial class Foto : ObservableObject
{
    [ObservableProperty] private long _imageId;

    [ObservableProperty] private Uri _imageUri;
}
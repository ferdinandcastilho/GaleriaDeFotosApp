using GaleriaDeFotos.Services;

namespace GaleriaCoreTests;

public class IoTests
{
    [Fact]
    public async Task ReadImagesFromFolder()
    {
        // Arrange
        FotosDataService fotosDataService = new(null); // No need for a valid DbContext
        string imageFolderPath = @"Z:\Users\Ferdi\OneDrive\Imagens\Saved Pictures\Wallpaper";

        // Act
        var imageFileList = await fotosDataService.GetImagesFromFolderAsync(imageFolderPath);

        // Assert
        Assert.True(imageFileList.Count() ==
                    19); // Replace by the number of png and jpg files in your choosen folder
    }
}
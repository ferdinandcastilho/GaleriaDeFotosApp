using GaleriaDeFotos.Services;

namespace GaleriaCoreTests;

/// <summary>
///     Testes de IO
/// </summary>
public class IoTests
{
    [Fact]
    public async Task ReadImagesFromFolder()
    {
        // Arrange
        FotosDataService fotosDataService = new(null);
        var imageFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        // Act
        var imageFileList = await fotosDataService.GetImagesFromFolderAsync(imageFolderPath);

        // Assert
        Assert.True(imageFileList.Count() == 19);
    }
}
using GaleriaDeFotos.Core.DataContext;
using Microsoft.EntityFrameworkCore;

namespace GaleriaCoreTests;

public class DatabaseTests
{
    /// <summary>
    ///     Testa Se as Imagens estão sendo lidas
    /// </summary>
    [Fact]
    public async Task ListAllPicturesData()
    {
        // Arrange 
        var options = new DbContextOptionsBuilder<FotoContext>()
            .UseInMemoryDatabase("FotoListDatabase").Options;

        // Act
        await using var context = new FotoContext(options);
        var listOfPictures = await context.Fotos.ToListAsync();

        // Assert
        Assert.Empty(listOfPictures);
    }
}
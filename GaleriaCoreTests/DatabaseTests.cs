using GaleriaDeFotos.Core.DataContext;
using GaleriaDeFotos.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GaleriaCoreTests;

public class DatabaseTests
{
    [Fact]
    public async Task ListAllPicturesData()
    {
        //TODO: Maikeu - Setup database connection
        // Arrange
        var options = new DbContextOptionsBuilder<FotoContext>()
            .UseInMemoryDatabase(databaseName: "FotoListDatabase")
            .Options;

        var configurator = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", true, true).Build();

        // Act
        await using var context = new FotoContext(options);
        var connectionString = configurator["ConnectionSqlite:SqliteConnectionString"];
        FotoContext.SetConnectionString(connectionString);
        List<FotoData> listOfPictures = await context.Fotos.ToListAsync();


        // Assert
        Assert.NotEmpty(listOfPictures);
    }


}
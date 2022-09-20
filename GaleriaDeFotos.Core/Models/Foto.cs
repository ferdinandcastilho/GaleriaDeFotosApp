using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace GaleriaDeFotos.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public partial class Foto : ObservableObject
{
    [ObservableProperty] private string _imageId;

    [ObservableProperty] private Uri _imageUri;

    public FotoData ToData()
    {
        var data = new FotoData { ImageId = ImageId, ImageUri = ImageUri.AbsolutePath };
        return data;
    }

    public void FromData(FotoData data)
    {
        ImageId = data.ImageId;
        ImageUri = new Uri(data.ImageUri);
    }
}

public class FotoData
{
    public string ImageId { get; set; }
    public string ImageUri { get; set; }
}

public class FotoContext : DbContext
{
    private static bool _created;
    private static string _connectionString;

    public FotoContext()
    {
        if (!_created)
        {
            _created = true;
            // ReSharper disable VirtualMemberCallInConstructor
            Database.EnsureDeleted();
            Database.EnsureCreated();
            // ReSharper restore VirtualMemberCallInConstructor
        }
    }

    public FotoContext(DbContextOptions<FotoContext> options) : base(options) { }

    public DbSet<FotoData> Fotos { get; set; }

    public static void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FotoData>().HasKey(m => m.ImageId);
        base.OnModelCreating(builder);
    }
}
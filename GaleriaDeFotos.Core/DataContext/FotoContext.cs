using GaleriaDeFotos.Core.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GaleriaDeFotos.Core.DataContext;

public sealed class FotoContext : DbContext
{
    private static bool _created;
    private static string _connectionString;

    public FotoContext()
    {
        if (_created) return;
        _created = true;
    }

    public FotoContext(DbContextOptions<FotoContext> options) : base(options) { }

    [UsedImplicitly]
    public DbSet<FotoData> Fotos
    {
        get; set;
    }

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

    public void EnsureCreated()
    {
        Database.EnsureCreated();
    }

    public void Recreate()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}
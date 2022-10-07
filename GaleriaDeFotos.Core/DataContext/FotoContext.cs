using System.Data.SQLite;
using System.Diagnostics;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Core.Services;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GaleriaDeFotos.Core.DataContext;

public sealed class FotoContext : DbContext
{
    private static string _connectionString;

    public FotoContext(DbContextOptions<FotoContext> options) : base(options)
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
            .Build();

        var dbPath = GetDbPath();

        if (!File.Exists(dbPath)) SQLiteConnection.CreateFile(dbPath);

        Database.EnsureCreated();
    }

    private IConfigurationRoot Configuration { get; }

    [UsedImplicitly] public DbSet<FotoData> Fotos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString).LogTo(message => Debug.WriteLine(message));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FotoData>().HasKey(m => m.ImageId);
        base.OnModelCreating(builder);
    }

    private string GetDbPath()
    {
        var connection = Configuration["ConnectionSqlite:SqliteConnectionString"];

        var connectionStringBuilder = new SQLiteConnectionStringBuilder(connection);
        var baseFolder = RuntimeConfigData.ApplicationFolder;

        var dbPath = Path.Combine(baseFolder, connectionStringBuilder.DataSource);

        connectionStringBuilder.DataSource = dbPath;
        _connectionString = connectionStringBuilder.ConnectionString;

        return dbPath;
    }

    public void Recreate()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}
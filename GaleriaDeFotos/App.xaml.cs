using System.Data.SQLite;
using Windows.Storage;
using GaleriaDeFotos.Activation;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.Settings;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Models;
using GaleriaDeFotos.Core.Services;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Models;
using GaleriaDeFotos.Services;
using GaleriaDeFotos.Services.Settings;
using GaleriaDeFotos.ViewModels;
using GaleriaDeFotos.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace GaleriaDeFotos;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App
{
    public App()
    {
        InitializeComponent();
        //Set AppSettings.json
        var builder = GetAppSettingsBuilder();
        Configuration = builder.Build();
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory).ConfigureServices((context, services) =>
            {
                // Default Activation Handler
                services
                    .AddTransient<ActivationHandler<LaunchActivatedEventArgs>,
                        DefaultActivationHandler>();

                // Other Activation Handlers

                // Settings
                services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddSingleton<LastFolderOptionSelectorService>();
                // Services

                services.AddTransient<INavigationViewService, NavigationViewService>();

                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IFotosDataService, FotosDataService>();
                services.AddSingleton<IFileService, FileService>();

                // Views and ViewModels
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsPage>();
                services.AddTransient<BlankViewModel>();
                services.AddTransient<BlankPage>();
                services.AddTransient<FavoritasViewModel>();
                services.AddTransient<FavoritasPage>();
                services.AddTransient<FotosDetailViewModel>();
                services.AddTransient<FotosDetailPage>();
                services.AddTransient<FotosFullPage>();
                services.AddTransient<FotosFullViewModel>();
                services.AddTransient<FotosViewModel>();
                services.AddTransient<FotosPage>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<MainPage>();
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();
                ConfigureDb(services);

                // Configuration
                services.Configure<LocalSettingsOptions>(
                    context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            }).Build();
        UnhandledException += App_UnhandledException;
        var db = GetService<FotoContext>();
        // db.EnsureCreated();
        db.Recreate();
    }

    public IConfigurationRoot Configuration { get; }

    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host { get; }

    public static WindowEx MainWindow { get; } = new MainWindow();

    private void ConfigureDb(IServiceCollection services)
    {
        var dbPath = GetConnectionString(out var connectionString);
        FotoContext.SetConnectionString(connectionString);
        if (!File.Exists(dbPath))
        {
            SQLiteConnection.CreateFile(dbPath);
            var _ = new FotoContext();
        }

        services.AddSqlite<FotoContext>(connectionString);
    }

    private string GetConnectionString(out string connectionString)
    {
        var connection = Configuration["ConnectionSqlite:SqliteConnectionString"];
        var connectionStringBuilder = new SQLiteConnectionStringBuilder(connection);
        var baseFolder = string.Empty;
        if (RuntimeHelper.IsMsix)
        {
            baseFolder = ApplicationData.Current.LocalFolder.Path;
        }

        var dbPath = Path.Combine(baseFolder, connectionStringBuilder.DataSource);
        connectionStringBuilder.DataSource = dbPath;
        connectionString = connectionStringBuilder.ConnectionString;
        return dbPath;
    }

    public static T GetService<T>() where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            throw new ArgumentException(
                $"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");

        return service;
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await GetService<IActivationService>().ActivateAsync(args);
    }

    private static IConfigurationBuilder GetAppSettingsBuilder()
    {
        return new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", true, true);
    }
}
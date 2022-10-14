using GaleriaDeFotos.Activation;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Contracts.Settings;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.DataContext;
using GaleriaDeFotos.Core.Services;
using GaleriaDeFotos.Models;
using GaleriaDeFotos.Services;
using GaleriaDeFotos.Services.Settings;
using GaleriaDeFotos.ViewModels;
using GaleriaDeFotos.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace GaleriaDeFotos;

/// <inheritdoc />
/// <summary>
///     Aplicação Principal
/// </summary>
public partial class App
{
    public App()
    {
        InitializeComponent();
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory).ConfigureServices((context, services) =>
            {
                // Default Activation Handler
                services
                    .AddTransient<ActivationHandler<LaunchActivatedEventArgs>,
                        DefaultActivationHandler>();

                // Settings
                services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                services.AddSingleton<LastFolderOptionSelectorService>();
                services.Configure<LocalSettingsOptions>(
                    context.Configuration.GetSection(nameof(LocalSettingsOptions)));

                // Services
                services.AddTransient<INavigationViewService, NavigationViewService>();
                services.AddDbContext<FotoContext>();

                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IFotosDataService, FotosDataService>();
                services.AddSingleton<IFileService, FileService>();

                // Views and ViewModels
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsPage>();
                services.AddTransient<AboutViewModel>();
                services.AddTransient<AboutPage>();
                services.AddTransient<FavoritasViewModel>();
                services.AddTransient<FavoritasPage>();
                services.AddTransient<FotosFullPage>();
                services.AddSingleton<FotosFullViewModel>();
                services.AddTransient<FotosViewModel>();
                services.AddTransient<FotosPage>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<MainPage>();
                services.AddTransient<ShellPage>();
                services.AddTransient<ShellViewModel>();
            }).Build();

        UnhandledException += App_UnhandledException;
    }

    public IHost Host { get; }

    public static WindowEx MainWindow { get; } = new MainWindow();

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

        var hWnd = WindowNative.GetWindowHandle(MainWindow);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);
        if (appWindow is null) return;
        var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);
        if (displayArea is null) return;
        var centeredPosition = appWindow.Position;
        centeredPosition.X = (displayArea.WorkArea.Width - appWindow.Size.Width) / 2;
        centeredPosition.Y = (displayArea.WorkArea.Height - appWindow.Size.Height) / 2;
        appWindow.Move(centeredPosition);
    }
}
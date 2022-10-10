namespace GaleriaDeFotos;

public sealed partial class MainWindow
{
    public static WindowEx? Instance;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "Galeria de Fotos";
        Instance = this;
    }
}
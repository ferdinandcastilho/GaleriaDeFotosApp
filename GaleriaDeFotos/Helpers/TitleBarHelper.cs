﻿using System.Runtime.InteropServices;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using WinRT.Interop;

namespace GaleriaDeFotos.Helpers;

/// <summary>
///     Helper class to workaround custom title bar bugs.
///     DISCLAIMER: The resource key names and color values used below are subject to change. Do not depend on them.
///     https://github.com/microsoft/TemplateStudio/issues/4516
/// </summary>
internal class TitleBarHelper
{
    private const int Wainactive = 0x00;
    private const int Waactive = 0x01;
    private const int Wmactivate = 0x0006;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    public static void UpdateTitleBar(ElementTheme theme)
    {
        if (App.MainWindow.ExtendsContentIntoTitleBar)
        {
            if (theme != ElementTheme.Default)
            {
                Application.Current.Resources["WindowCaptionForeground"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(Colors.White),
                    ElementTheme.Light => new SolidColorBrush(Colors.Black),
                    _ => new SolidColorBrush(Colors.Transparent)
                };

                Application.Current.Resources["WindowCaptionForegroundDisabled"] = theme switch
                {
                    ElementTheme.Dark =>
                        new SolidColorBrush(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                    ElementTheme.Light => new SolidColorBrush(
                        Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                    _ => new SolidColorBrush(Colors.Transparent)
                };

                Application.Current.Resources["WindowCaptionButtonBackgroundPointerOver"] =
                    theme switch
                    {
                        ElementTheme.Dark => new SolidColorBrush(Color.FromArgb(0x33, 0xFF, 0xFF,
                            0xFF)),
                        ElementTheme.Light => new SolidColorBrush(Color.FromArgb(0x33, 0x00, 0x00,
                            0x00)),
                        _ => new SolidColorBrush(Colors.Transparent)
                    };

                Application.Current.Resources["WindowCaptionButtonBackgroundPressed"] = theme switch
                {
                    ElementTheme.Dark =>
                        new SolidColorBrush(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                    ElementTheme.Light => new SolidColorBrush(
                        Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                    _ => new SolidColorBrush(Colors.Transparent)
                };

                Application.Current.Resources["WindowCaptionButtonStrokePointerOver"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(Colors.White),
                    ElementTheme.Light => new SolidColorBrush(Colors.Black),
                    _ => new SolidColorBrush(Colors.Transparent)
                };

                Application.Current.Resources["WindowCaptionButtonStrokePressed"] = theme switch
                {
                    ElementTheme.Dark => new SolidColorBrush(Colors.White),
                    ElementTheme.Light => new SolidColorBrush(Colors.Black),
                    _ => new SolidColorBrush(Colors.Transparent)
                };
            }

            Application.Current.Resources["WindowCaptionBackground"] =
                new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["WindowCaptionBackgroundDisabled"] =
                new SolidColorBrush(Colors.Transparent);

            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
            if (hwnd == GetActiveWindow())
            {
                SendMessage(hwnd, Wmactivate, Wainactive, IntPtr.Zero);
                SendMessage(hwnd, Wmactivate, Waactive, IntPtr.Zero);
            } else
            {
                SendMessage(hwnd, Wmactivate, Waactive, IntPtr.Zero);
                SendMessage(hwnd, Wmactivate, Wainactive, IntPtr.Zero);
            }
        }
    }
}
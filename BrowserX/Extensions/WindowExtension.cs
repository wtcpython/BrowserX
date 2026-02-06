using BrowserX.Models;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using BrowserX.Helpers;

namespace BrowserX.Extensions;

public static class WindowExtension
{
    extension(Window window)
    {
        public void SetBackdrop()
        {
            var effect = App.Profile.BackgroundEffect;
            window.SystemBackdrop = effect switch
            {
                Effect.Mica when MicaController.IsSupported() => new MicaBackdrop(),
                Effect.MicaAlt when MicaController.IsSupported() => new MicaBackdrop { Kind = MicaKind.BaseAlt },
                Effect.Acrylic => new DesktopAcrylicBackdrop(),
                _ => null
            };
        }

        public void SetThemeColor()
        {
            var appearance = App.Profile.Appearance;
            if (window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Enum.Parse<ElementTheme>(appearance);
                appearance = appearance == "Default" ? "UseDefaultAppMode" : appearance;
                window.AppWindow.TitleBar.PreferredTheme = Enum.Parse<TitleBarTheme>(appearance);
            }
        }

        public IntPtr GetWindowHandle()
        {
            return Win32Interop.GetWindowFromWindowId(window.AppWindow.Id);
        }
    }

    extension(UIElement element)
    {
        public WindowId GetWindowId()
        {
            var window = WindowHelper.GetWindowForElement(element);
            return window?.AppWindow.Id ?? new WindowId();
        }
    }
}
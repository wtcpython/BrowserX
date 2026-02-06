using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BrowserX.Helpers;

public class WindowHelper
{
    public static List<MainWindow> MainWindows = [];

    public static MainWindow CreateWindow()
    {
        var window = new MainWindow();
        window.Closed += (sender, args) =>
        {
            MainWindows.Remove(window);
            if (MainWindows.Count == 0)
            {
                File.WriteAllText("./profile.json",
                    JsonSerializer.Serialize(App.Profile, JsonHelper.JsonContext.Default.Profile));
            }
        };
        MainWindows.Add(window);
        return window;
    }

    public static MainWindow? GetWindowForElement(UIElement? element)
    {
        if (element?.XamlRoot != null)
        {
            foreach (var window in MainWindows)
                if (element.XamlRoot == window.Content.XamlRoot)
                    return window;
        }

        return null;
    }
}
using BrowserX.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BrowserX.Helpers;

public class WebViewHelper
{
    public static WebView2? GetWebView(UIElement? element)
    {
        var page = WindowHelper.GetWindowForElement(element)?.SelectedItem as WebViewPage;
        return page?.WebView2;
    }

    public static bool AnyWebviewPageExists()
    {
        foreach (var mainWindow in WindowHelper.MainWindows)
        foreach (var tabItem in mainWindow.TabView.TabItems)
            if (tabItem is TabViewItem { Content: WebViewPage })
                return true;

        return false;
    }
}
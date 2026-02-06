using BrowserX.Pages;
using BrowserX.Settings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System.Linq;
using BrowserX.Helpers;

namespace BrowserX.Controls;

public sealed partial class BrowserMenuItems : UserControl
{
    public BrowserMenuItems()
    {
        InitializeComponent();
    }

    private void TryCreateNewTab(object sender, RoutedEventArgs e)
    {
        var mainWindow = WindowHelper.GetWindowForElement(this);
        mainWindow?.AddHomePage();
    }

    private void TryCreateNewWindow(object sender, RoutedEventArgs e)
    {
        var window = WindowHelper.CreateWindow();
        window.AddHomePage();
        window.Activate();
    }

    private void TryOpenSettingPage(object sender, RoutedEventArgs e)
    {
        var mainWindow = WindowHelper.GetWindowForElement(this);
        var tabView = mainWindow?.Content as TabView;

        var item = tabView?.TabItems.FirstOrDefault(x => ((TabViewItem)x).Content is SettingsPage);
        if (item != null) tabView?.SelectedItem = item;
        else mainWindow?.AddNewTab(new SettingsPage(), "设置", new FontIconSource { Glyph = "\ue713" });
    }

    private void ShowFlyout(object sender, RoutedEventArgs e)
    {
        var page = WindowHelper.GetWindowForElement(this)?.SelectedItem as WebViewPage;
        page?.ShowFlyout(((MenuFlyoutItem)sender).Text);
    }

    private void ShowPrintUi(object sender, RoutedEventArgs e)
    {
        var webView2 = WebViewHelper.GetWebView(this);
        webView2?.CoreWebView2.ShowPrintUI(CoreWebView2PrintDialogKind.Browser);
    }

    private void CloseApp(object sender, RoutedEventArgs e)
    {
        WindowHelper.MainWindows.ToList().ForEach(x => x.Close());
    }

    private void MenuFlyout_Opening(object sender, object e)
    {
        var items = ((MenuFlyout)sender).Items.ToList()[2..^3];
        var mainWindow = WindowHelper.GetWindowForElement(this);
        if (mainWindow?.SelectedItem is WebViewPage)
            items.ForEach(x => x.Visibility = Visibility.Visible);
        else
            items.ForEach(x => x.Visibility = Visibility.Collapsed);
    }
}
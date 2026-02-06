using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace BrowserX.Settings.System;

public sealed partial class SystemMainPage : Page
{
    public SystemMainPage()
    {
        InitializeComponent();
    }

    private void JumpToSystemItem(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(SystemItem));
        SettingsPage.BreadcrumbBarItems.Add(new NavigationItem
        {
            Header = "系统",
            Type = typeof(SystemItem)
        });
    }

    private void JumpToPerformanceItem(object sender, RoutedEventArgs e)
    {
    }

    private async void OpenSettingsPrinter(object sender, RoutedEventArgs e)
    {
        await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:printers"));
    }
}
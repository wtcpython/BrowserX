using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BrowserX.Settings.Privacy;

public sealed partial class PrivacyMainPage : Page
{
    public PrivacyMainPage()
    {
        InitializeComponent();
    }

    private void JumpToTrackItem(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(TrackItem));
        SettingsPage.BreadcrumbBarItems.Add(new NavigationItem
        {
            Header = "跟踪防护",
            Type = typeof(TrackItem)
        });
    }

    private void JumpToClearDataItem(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ClearDataItem));
        SettingsPage.BreadcrumbBarItems.Add(new NavigationItem
        {
            Header = "清除浏览数据",
            Type = typeof(ClearDataItem)
        });
    }

    private void JumpToSafetyItem(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(SafetyItem));
        SettingsPage.BreadcrumbBarItems.Add(new NavigationItem
        {
            Header = "安全性",
            Type = typeof(SafetyItem)
        });
    }
}
using System;
using System.Collections.Generic;
using BrowserX.Helpers;
using BrowserX.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BrowserX.Settings.System;

public sealed partial class SystemItem : Page
{
    public SystemItem()
    {
        InitializeComponent();
        SetToggleEnableGpu.IsOn = !App.Profile.DisableGpu;
        RestartInfoBar.IsOpen = App.NeedRestartEnvironment;
    }

    private void ToggleEnableGpu(object sender, RoutedEventArgs e)
    {
        if (App.Profile.DisableGpu == SetToggleEnableGpu.IsOn)
        {
            App.NeedRestartEnvironment = true;
            App.Profile.DisableGpu = !SetToggleEnableGpu.IsOn;
            RestartInfoBar.IsOpen = true;
        }
    }

    private async void OpenSettingsProxy(object sender, RoutedEventArgs e)
    {
        await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:network-proxy"));
    }

    private void CloseAllWebviews(object sender, RoutedEventArgs e)
    {
        if (App.NeedRestartEnvironment)
        {
            List<TabViewItem> tabs = [];
            foreach (var window in WindowHelper.MainWindows)
            {
                foreach (var tabItem in window.TabView.TabItems)
                    if (tabItem is TabViewItem { Content: WebViewPage webViewPage } tabViewItem)
                    {
                        webViewPage.Close();
                        tabs.Add(tabViewItem);
                    }

                foreach (var tabViewItem in tabs) window.TabView.TabItems.Remove(tabViewItem);
            }

            App.WebView2.Close();
        }

        RestartInfoBar.IsOpen = false;
    }
}
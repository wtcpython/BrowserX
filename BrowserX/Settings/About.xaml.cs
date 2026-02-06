using BrowserX.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using ReleaseInfo = Microsoft.Windows.ApplicationModel.WindowsAppRuntime.ReleaseInfo;

namespace BrowserX.Settings;

public sealed partial class About : Page
{
    public string appVersion = File.ReadAllText("./Assets/version.txt");
    public string webViewVersion = CoreWebView2Environment.GetAvailableBrowserVersionString();

    public List<OpenSource> OpenSources = [];

    public About()
    {
        InitializeComponent();
        OpenSources =
        [
            new OpenSource("https://github.com/CommunityToolkit/Windows", "Windows Community Toolkit"),
            new OpenSource("https://aka.ms/webview", "Microsoft.Web.WebView2"),
            new OpenSource("https://github.com/microsoft/windowsappsdk",
                $"Microsoft.WindowsAppSDK {ReleaseInfo.AsString}"),
            new OpenSource("https://aka.ms/WinSDKProjectURL", "Microsoft.Windows.SDK.BuildTools")
        ];
    }

    private static void CopyText(string text)
    {
        DataPackage package = new();
        package.SetText(text);
        Clipboard.SetContent(package);
    }

    private void CopyWebViewVersion(object sender, RoutedEventArgs e)
    {
        CopyText(webViewVersion);
    }

    private void CopyAppVersion(object sender, RoutedEventArgs e)
    {
        CopyText(appVersion);
    }

    private async void OpenRepoWebsite(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/wtcpython/WinUIEdge"));
    }

    private async void CheckUpdate(object sender, RoutedEventArgs e)
    {
        try
        {
            var fileUri = "https://raw.githubusercontent.com/wtcpython/WinUIEdge/main/Assets/version.txt";
            using HttpClient client = new();
            var version = await client.GetStringAsync(fileUri);

            if (version.CompareTo(appVersion) > 0)
            {
                var builder = new AppNotificationBuilder()
                    .AddText($"发现新版本：{version}，是否要更新？\n当前版本：{appVersion}")
                    .AddArgument("Notification", "LaunchReleaseWebsite")
                    .AddButton(new AppNotificationButton("确定")
                        .AddArgument("Notification", "LaunchReleaseWebsite"))
                    .AddButton(new AppNotificationButton("取消"));

                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
            }
        }
        catch (Exception)
        {
        }
    }
}
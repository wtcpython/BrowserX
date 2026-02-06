using BrowserX.Extensions;
using BrowserX.Helpers;
using BrowserX.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrowserX.Settings;

public sealed partial class StartItem : Page
{
    public Profile Profile => App.Profile;

    public List<string> SearchEngines { get; } = ProfileHelper.SearchEngineList.Select(x => x.Name).ToList();

    public bool IsHomeButtonOn
    {
        get => Profile.ToolBar.GetValueOrDefault("HomeButton", false);
        set => Profile.ToolBar["HomeButton"] = value;
    }

    public StartItem()
    {
        InitializeComponent();
    }

    public Visibility IsCustomWebsiteVisible(int index)
    {
        return index == 2 ? Visibility.Visible : Visibility.Collapsed;
    }

    public Visibility IsCustomBackgroundVisible(int index)
    {
        return index == 2 ? Visibility.Visible : Visibility.Collapsed;
    }

    private void CheckUri(object sender, RoutedEventArgs e)
    {
        var uriType = UriBox.Text.DetectUri();
        if (uriType == UriType.WithoutProtocol)
        {
            UriBox.Text = "https://" + UriBox.Text;
            uriType = UriType.WithProtocol;
        }

        if (uriType != UriType.WithProtocol)
        {
            var builder = new AppNotificationBuilder().AddText("输入的 Uri 非法，请修改内容")
                .AddArgument("Notification", "ChangeStartUri");
            var notificationManager = AppNotificationManager.Default;
            notificationManager.Show(builder.BuildNotification());
        }
        else App.Profile.SpecificUri = UriBox.Text;
    }

    private void WebsiteBehaviorChanged(object sender, SelectionChangedEventArgs e)
    {
        var radio = sender as RadioButtons;
        if (radio!.SelectedIndex != 2)
        {
            // 清空绑定属性
            Profile.SpecificUri = string.Empty;

            // 清空输入框显示
            UriBox.Text = string.Empty;
        }
    }

    private void BackgroundBehaviorChanged(object sender, SelectionChangedEventArgs e)
    {
        var radio = sender as RadioButtons;
        if (radio!.SelectedIndex != 2)
        {
            // 清空绑定属性
            Profile.BackgroundLocation = string.Empty;

            // 清空输入框显示
            BackgroundBox.Text = string.Empty;
        }
    }

    private async void SetBackgroundImage(object sender, RoutedEventArgs e)
    {
        FileOpenPicker picker = new(this.GetWindowId())
        {
            SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            FileTypeFilter = { { ".jpg" }, { ".jpeg" }, { ".png" } }
        };

        var result = await picker.PickSingleFileAsync();

        if (result != null) Profile.BackgroundLocation = result.Path;
    }
}
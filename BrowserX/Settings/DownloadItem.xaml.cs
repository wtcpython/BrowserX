using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;
using System;
using BrowserX.Extensions;
using BrowserX.Models;
using Microsoft.Web.WebView2.Core;

namespace BrowserX.Settings;

public sealed partial class DownloadItem : Page
{
    public Profile Profile => App.Profile;

    public CoreWebView2Profile CoreWebView2Profile => App.CoreWebView2Profile;

    public DownloadItem()
    {
        InitializeComponent();
    }

    private async void ChangeDownloadFolder(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FolderPicker picker = new(this.GetWindowId())
        {
            SuggestedStartLocation = PickerLocationId.Downloads
        };

        var result = await picker.PickSingleFolderAsync();
        if (result != null)
        {
            CoreWebView2Profile.DefaultDownloadFolderPath = result.Path;
            DownloadFolderCard.Description = result.Path;
        }
    }
}
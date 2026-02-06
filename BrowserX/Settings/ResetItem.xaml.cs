using BrowserX.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using Windows.Storage;

namespace BrowserX.Settings;

public sealed partial class ResetItem : Page
{
    public ResetItem()
    {
        InitializeComponent();
    }

    private async void ResetUserSettings(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var res = await ResetDialog.ShowAsync();
        if (res == ContentDialogResult.Primary)
        {
            var path = ApplicationData.Current.LocalFolder.Path + "/profile.json";
            File.Delete(path);
            App.Profile = ProfileHelper.LoadProfile(true);
        }
    }
}
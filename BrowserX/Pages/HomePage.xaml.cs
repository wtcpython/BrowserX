using BrowserX.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.IO;
using BrowserX.Helpers;

namespace BrowserX.Pages;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        Loaded += InstallWebView2;

        View.ItemsSource = ProfileHelper.Favorites;

        var location = App.Profile.BackgroundLocation;
        if (App.Profile.BackgroundBehavior == 2 && File.Exists(location))
        {
            Background = new ImageBrush
            {
                ImageSource = new BitmapImage
                {
                    UriSource = new Uri(App.Profile.BackgroundLocation)
                },
                Stretch = Stretch.UniformToFill
            };
        }
        else if (App.Profile.BackgroundBehavior is 1 or 2) LoadBingImage();

        // if (App.Profile.ShowBackground)
        // {
        //     if (!string.IsNullOrEmpty(App.Profile.BackgroundImage))
        //     {
        //         Background = new ImageBrush
        //         {
        //             ImageSource = new BitmapImage
        //             {
        //                 UriSource = new Uri(App.Profile.BackgroundImage)
        //             },
        //             Stretch = Stretch.UniformToFill
        //         };
        //     }
        //     else
        //         LoadBingImage();
        // }

        FavoriteList.Visibility = App.Profile.MenuStatus != "Never" ? Visibility.Visible : Visibility.Collapsed;
    }

    private async void LoadBingImage()
    {
        var url = await Utilities.GetBingImageUrlAsync();
        Background = new ImageBrush
        {
            ImageSource = new BitmapImage
            {
                UriSource = new Uri(url)
            },
            Stretch = Stretch.UniformToFill
        };
    }

    private async void InstallWebView2(object sender, RoutedEventArgs e)
    {
        var version = CoreWebView2Environment.GetAvailableBrowserVersionString();
        var bootstrapUri = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
        if (!string.IsNullOrEmpty(version)) return;
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "winget",
                Arguments = $"install --id=Microsoft.EdgeWebView2Runtime -e --silent",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception)
        {
            await warningDialog.ShowAsync();
            await Windows.System.Launcher.LaunchUriAsync(new Uri(bootstrapUri));
            WindowHelper.GetWindowForElement(this)?.Close();
        }
    }

    private void OpenFavoriteWebsite(object sender, ItemClickEventArgs e)
    {
        var mainWindow = WindowHelper.GetWindowForElement(this);
        mainWindow?.AddNewTab(new WebViewPage((e.ClickedItem as WebsiteInfo)!.Uri));
    }

    private void WebSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var text = args.QueryText;
        sender.Text = string.Empty;
        var mainWindow = WindowHelper.GetWindowForElement(this);
        Utilities.Search(text, mainWindow);
    }
}
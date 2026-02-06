using BrowserX.Helpers;
using BrowserX.Models;
using BrowserX.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;

namespace BrowserX.Controls;

public sealed partial class FavoriteList : UserControl
{
    public ItemsPanelTemplate HorizontalTemplate => horizontalTemplate;
    public ItemsPanelTemplate VerticalTemplate => verticalTemplate;


    public FavoriteList()
    {
        this.InitializeComponent();
        SetItemsPanel(HorizontalTemplate);
        ListView.ItemsSource = ProfileHelper.Favorites;
    }

    public void SetItemsPanel(ItemsPanelTemplate itemsPanelTemplate)
    {
        ListView.ItemsPanel = itemsPanelTemplate;
        if (itemsPanelTemplate == HorizontalTemplate)
        {
            Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }
        else
        {
            Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
    }

    public ItemsPanelTemplate ItemsPanel
    {
        get => ListView.ItemsPanel;
        set => ListView.ItemsPanel = value;
    }

    public ObservableCollection<WebsiteInfo> ItemsSource
    {
        get => (ObservableCollection<WebsiteInfo>)ListView.ItemsSource;
        set => ListView.ItemsSource = value;
    }

    private void OpenFavoriteWebsite(object sender, ItemClickEventArgs e)
    {
        OpenWebSite(((WebsiteInfo)e.ClickedItem).Uri);
    }

    private void OpenFavoriteWebsiteInNewTab(object sender, RoutedEventArgs e)
    {
        Uri uri = ((WebsiteInfo)((MenuFlyoutItem)sender).DataContext).Uri;
        OpenWebSite(uri);
    }

    private void OpenWebSite(Uri uri)
    {
        var mainWindow = WindowHelper.GetWindowForElement(this);
        mainWindow?.AddNewTab(new WebViewPage(uri));
    }

    private void OpenFavoriteWebsiteInNewWindow(object sender, RoutedEventArgs e)
    {
        var uri = ((WebsiteInfo)((MenuFlyoutItem)sender).DataContext).Uri;
        var window = WindowHelper.CreateWindow();
        window.AddNewTab(new WebViewPage(uri));
        window.Activate();
    }

    private void CopyFavoriteWebsite(object sender, RoutedEventArgs e)
    {
        var uri = ((WebsiteInfo)((MenuFlyoutItem)sender).DataContext).Uri;
        DataPackage package = new();
        package.SetText(uri.ToString());
        Clipboard.SetContent(package);
    }

    private void DeleteFavoriteWebsite(object sender, RoutedEventArgs e)
    {
        var info = (WebsiteInfo)((MenuFlyoutItem)sender).DataContext;
        ProfileHelper.Favorites.Remove(info);
    }
}
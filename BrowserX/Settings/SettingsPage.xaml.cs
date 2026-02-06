using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BrowserX.Pages;
using Microsoft.UI.Xaml.Input;

namespace BrowserX.Settings;

public class NavigationItem
{
    public string Header { get; set; }
    public Type Type { get; set; }
}

public sealed partial class SettingsPage : Page
{
    public static ObservableCollection<NavigationItem> BreadcrumbBarItems = [];

    private static readonly Dictionary<string, Type> NavigationTypeMap = new()
    {
        { "Privacy.PrivacyMainPage", typeof(Privacy.PrivacyMainPage) },
        { "Appearance.AppearanceMainPage", typeof(Appearance.AppearanceMainPage) },
        { "StartItem", typeof(StartItem) },
        { "DownloadItem", typeof(DownloadItem) },
        { "System.SystemMainPage", typeof(System.SystemMainPage) },
        { "ResetItem", typeof(ResetItem) },
        { "About", typeof(About) }
    };

    public SettingsPage()
    {
        InitializeComponent();
        Navigation.SelectedItem = Navigation.MenuItems[0];
        BreadcrumbBar.ItemsSource = BreadcrumbBarItems;
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var tag = (string)(args.SelectedItem as NavigationViewItem)?.Tag;

        // If we have a valid tag, update Breadcrumb and navigate
        if (!string.IsNullOrEmpty(tag) && NavigationTypeMap.ContainsKey(tag))
        {
            UpdateBreadcrumb(new NavigationItem
            {
                Header = ((NavigationViewItem)Navigation.SelectedItem)?.Content?.ToString(),
                Type = NavigationTypeMap[tag]
            });
        }
    }

    public void Navigate(string tag)
    {
        var menuItem =
            Navigation.MenuItems.FirstOrDefault(x => (string)(x as NavigationViewItem)?.Tag == tag) as
                NavigationViewItem;

        if (menuItem != null)
        {
            Navigation.SelectedItem = menuItem;
            UpdateBreadcrumb(new NavigationItem
            {
                Header = menuItem.Content.ToString()!,
                Type = NavigationTypeMap.GetValueOrDefault(tag)
            });
        }
    }

    private void UpdateBreadcrumb(NavigationItem item)
    {
        BreadcrumbBarItems.Clear();
        BreadcrumbBarItems.Add(item);

        ContentFrame.Navigate(item.Type);
    }

    private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        for (var i = BreadcrumbBarItems.Count - 1; i >= args.Index + 1; i--)
            BreadcrumbBarItems.RemoveAt(i);

        ContentFrame.Navigate(BreadcrumbBarItems[args.Index].Type);
    }

    private void Release_Tapped(object sender, TappedRoutedEventArgs e)
    {
        if (App.ReleaseWindow == null)
            App.ReleaseWindow = new ReleaseWindow();

        App.ReleaseWindow.Activate();
    }
}
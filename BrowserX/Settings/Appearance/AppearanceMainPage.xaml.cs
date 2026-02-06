using BrowserX.Extensions;
using BrowserX.Helpers;
using BrowserX.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace BrowserX.Settings.Appearance;

public sealed partial class AppearanceMainPage : Page
{
    private static bool inLoading = false;

    public List<string> themeList = [.. Enum.GetNames<ElementTheme>()];
    public List<string> effects = [.. Enum.GetNames<Effect>()];

    public AppearanceMainPage()
    {
        InitializeComponent();
        EffectBox.ItemsSource = effects;

        inLoading = true;
        AppearanceView.SelectedIndex = themeList.IndexOf(App.Profile.Appearance);
        EffectBox.SelectedIndex = effects.IndexOf(App.Profile.BackgroundEffect.ToString());
        inLoading = false;
    }

    private void AppearanceChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!inLoading)
        {
            var index = AppearanceView.SelectedIndex;
            App.Profile.Appearance = themeList[index];

            foreach (var window in WindowHelper.MainWindows) window.SetThemeColor();
            App.ReleaseWindow?.SetThemeColor();
        }
    }

    private void EffectChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!inLoading)
        {
            App.Profile.BackgroundEffect = Enum.Parse<Effect>((sender as ComboBox)!.SelectedItem.ToString()!);

            foreach (var window in WindowHelper.MainWindows) window.SetBackdrop();
            App.ReleaseWindow?.SetBackdrop();
        }
    }

    private void JumpToToolBarItem(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(ToolBarItem));
        SettingsPage.BreadcrumbBarItems.Add(new NavigationItem
        {
            Header = "工具栏",
            Type = typeof(ToolBarItem)
        });
    }
}
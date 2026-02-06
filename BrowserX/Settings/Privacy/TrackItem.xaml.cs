using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;

namespace BrowserX.Settings.Privacy;

public sealed partial class TrackItem : Page
{
    public List<string> Tracks = ["None", "基本", "平衡（推荐）", "严格"];

    public List<CoreWebView2TrackingPreventionLevel> TrackLevelList =
        [.. Enum.GetValues<CoreWebView2TrackingPreventionLevel>()];

    public TrackItem()
    {
        InitializeComponent();
        trackBox.ItemsSource = Tracks;

        var level = App.CoreWebView2Profile.PreferredTrackingPreventionLevel;
        trackBox.SelectedIndex = TrackLevelList.IndexOf(level);
    }

    private void TrackLevelChanged(object sender, SelectionChangedEventArgs e)
    {
        App.CoreWebView2Profile.PreferredTrackingPreventionLevel = TrackLevelList[trackBox.SelectedIndex];
    }
}
using BrowserX.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BrowserX.Helpers;
using BrowserX.ViewModels;

namespace BrowserX.Pages;

public sealed partial class ReleasePage : Page
{
    public IList<IList<ReleaseInfo>> ReleaseGroups { get; private set; }

    private bool _isLoading;

    public ReleasePage()
    {
        InitializeComponent();
    }

    private async void ShellPage_Loaded(object sender, RoutedEventArgs e)
    {
        SetTitleBar();
        await LoadReleasesAsync();
    }

    private async Task LoadReleasesAsync()
    {
        if (_isLoading) return;

        _isLoading = true;
        LoadingProgressRing.Visibility = Visibility.Visible;
        ErrorInfoBar.IsOpen = false;
        navigationView.MenuItems.Clear();

        try
        {
            var releases = await FetchReleasesFromGitHubAsync();
            ReleaseGroups = GroupReleasesByMajorMinor(releases);
            PopulateNavigationItems();
        }
        catch (Exception ex)
        {
            // Logger.LogError("Failed to load releases", ex);
            ErrorInfoBar.IsOpen = true;
        }
        finally
        {
            LoadingProgressRing.Visibility = Visibility.Collapsed;
            _isLoading = false;
        }
    }

    private static async Task<IList<ReleaseInfo>> FetchReleasesFromGitHubAsync()
    {
        using var proxyClientHandler = new HttpClientHandler
        {
            DefaultProxyCredentials = CredentialCache.DefaultCredentials,
            Proxy = WebRequest.GetSystemWebProxy(),
            PreAuthenticate = true
        };

        using var httpClient = new HttpClient(proxyClientHandler);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "WinUIEdge");

        var json = await httpClient.GetStringAsync(
            "https://api.github.com/repos/wtcpython/WinUIEdge/releases?per_page=20");
        var allReleases = JsonSerializer.Deserialize<IList<ReleaseInfo>>(json,
            JsonHelper.JsonContext.Default.IListReleaseInfo)!;

        return allReleases
            .OrderByDescending(r => r.PublishedDate)
            .ToList();
    }

    private static IList<IList<ReleaseInfo>> GroupReleasesByMajorMinor(IList<ReleaseInfo> releases)
    {
        return releases
            .GroupBy(r => GetMajorMinorVersion(r))
            .Select(g => g.OrderByDescending(r => r.PublishedDate).ToList() as IList<ReleaseInfo>)
            .ToList();
    }

    private static string GetMajorMinorVersion(ReleaseInfo release)
    {
        var version = GetVersionFromRelease(release);
        var parts = version.Split('.');
        if (parts.Length >= 2) return $"{parts[0]}.{parts[1]}";

        return version;
    }

    private static string GetVersionFromRelease(ReleaseInfo release)
    {
        // TagName is typically like "v0.96.0", Name might be "Release v0.96.0"
        var version = release.TagName ?? release.Name ?? "Unknown";
        if (version.StartsWith("v", StringComparison.OrdinalIgnoreCase)) version = version.Substring(1);

        return version;
    }

    private void PopulateNavigationItems()
    {
        if (ReleaseGroups == null || ReleaseGroups.Count == 0) return;

        foreach (var releaseGroup in ReleaseGroups)
        {
            var viewModel = new ReleaseGroupViewModel(releaseGroup);
            navigationView.MenuItems.Add(viewModel);
        }

        // Select the first item to trigger navigation
        navigationView.SelectedItem = navigationView.MenuItems[0];
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is ReleaseGroupViewModel viewModel)
            NavigationFrame.Navigate(typeof(ReleaseNotesPage), viewModel.Releases);
    }

    private async void RetryButton_Click(object sender, RoutedEventArgs e)
    {
        await LoadReleasesAsync();
    }

    private void SetTitleBar()
    {
        var window = App.ReleaseWindow;
        if (window != null)
        {
            window.ExtendsContentIntoTitleBar = true;
            window.SetTitleBar(AppTitleBar);
        }
    }

    private void NavigationView_DisplayModeChanged(NavigationView sender,
        NavigationViewDisplayModeChangedEventArgs args)
    {
        if (args.DisplayMode is NavigationViewDisplayMode.Compact or NavigationViewDisplayMode.Minimal)
        {
            TitleBarIcon.Margin = new Thickness(0, 0, 8, 0); // Workaround, see XAML comment
            AppTitleBar.IsPaneToggleButtonVisible = true;
        }
        else
        {
            TitleBarIcon.Margin = new Thickness(16, 0, 0, 0); // Workaround, see XAML comment
            AppTitleBar.IsPaneToggleButtonVisible = false;
        }
    }

    private void TitleBar_PaneButtonClick(TitleBar sender, object args)
    {
        navigationView.IsPaneOpen = !navigationView.IsPaneOpen;
    }
}
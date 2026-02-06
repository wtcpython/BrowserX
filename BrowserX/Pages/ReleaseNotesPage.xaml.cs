using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BrowserX.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BrowserX.Pages;

public sealed partial class ReleaseNotesPage : Page
{
    private IList<ReleaseInfo> _currentReleases;

    public ReleaseNotesPage()
    {
        InitializeComponent();
    }

    private static readonly CompositeFormat GitHubReleaseLinkTemplate =
        CompositeFormat.Parse("https://github.com/wtcpython/WinUIEdge/releases/tag/{0}");

    private static string ProcessReleaseNotesMarkdown(IList<ReleaseInfo> releases)
    {
        if (releases == null || releases.Count == 0) return string.Empty;

        var releaseNotesHtmlBuilder = new StringBuilder(string.Empty);

        var isFirst = true;
        foreach (var release in releases)
        {
            // Add separator between releases
            if (!isFirst)
            {
                releaseNotesHtmlBuilder.AppendLine("---");
                releaseNotesHtmlBuilder.AppendLine();
            }

            isFirst = false;

            var releaseUrl = string.Format(CultureInfo.InvariantCulture, GitHubReleaseLinkTemplate, release.TagName);
            releaseNotesHtmlBuilder.AppendLine(CultureInfo.InvariantCulture, $"# {release.Name}");
            releaseNotesHtmlBuilder.AppendLine(CultureInfo.InvariantCulture,
                $"{release.PublishedDate.ToString("D", CultureInfo.CurrentCulture)} • [View on GitHub]({releaseUrl})");
            releaseNotesHtmlBuilder.AppendLine();
            releaseNotesHtmlBuilder.AppendLine("&nbsp;");
            releaseNotesHtmlBuilder.AppendLine();
            var notes = release.ReleaseNotes;

            releaseNotesHtmlBuilder.AppendLine(notes);
            releaseNotesHtmlBuilder.AppendLine("&nbsp;");
        }

        return releaseNotesHtmlBuilder.ToString();
    }

    private void DisplayReleaseNotes()
    {
        if (_currentReleases == null || _currentReleases.Count == 0)
        {
            ReleaseNotesMarkdown.Visibility = Visibility.Collapsed;
            return;
        }

        try
        {
            LoadingProgressRing.Visibility = Visibility.Collapsed;

            ReleaseNotesMarkdown.Text = ProcessReleaseNotesMarkdown(_currentReleases);
            ReleaseNotesMarkdown.Visibility = Visibility.Visible;
        }
        catch
        {
            // Logger.LogError("Exception when displaying the release notes", ex);
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        DisplayReleaseNotes();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is IList<ReleaseInfo> releases) _currentReleases = releases;
    }
}
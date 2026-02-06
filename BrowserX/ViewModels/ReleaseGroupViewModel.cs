using System;
using System.Collections.Generic;
using System.Globalization;
using BrowserX.Models;

namespace BrowserX.ViewModels;

/// <summary>
/// View model for a group of releases (grouped by major.minor version).
/// </summary>
public class ReleaseGroupViewModel
{
    /// <summary>
    /// Gets the list of releases in this group.
    /// </summary>
    public IList<ReleaseInfo> Releases { get; }

    /// <summary>
    /// Gets the version text to display (e.g., "0.96.0").
    /// </summary>
    public string VersionText { get; }

    /// <summary>
    /// Gets the date text to display (e.g., "December 2025").
    /// </summary>
    public string DateText { get; }

    public ReleaseGroupViewModel(IList<ReleaseInfo> releases)
    {
        Releases = releases ?? throw new ArgumentNullException(nameof(releases));

        if (releases.Count > 0)
        {
            var latestRelease = releases[0];
            VersionText = GetVersionFromRelease(latestRelease);
            DateText = latestRelease.PublishedDate.ToString("Y", CultureInfo.CurrentCulture);
        }
        else
        {
            VersionText = "Unknown";
            DateText = string.Empty;
        }
    }

    private static string GetVersionFromRelease(ReleaseInfo release)
    {
        // TagName is typically like "v0.96.0", Name might be "Release v0.96.0"
        var version = release.TagName ?? release.Name ?? "Unknown";
        if (version.StartsWith("v", StringComparison.OrdinalIgnoreCase)) version = version[1..];

        return version;
    }
}
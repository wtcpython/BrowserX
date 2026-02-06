using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace BrowserX.Models;

public partial class Profile : ObservableObject
{
    [ObservableProperty]
    public partial string Appearance { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool AskDownloadBehavior { get; set; }

    [ObservableProperty]
    public partial Effect BackgroundEffect { get; set; }

    // [ObservableProperty]
    // public partial string BackgroundImage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string MenuStatus { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SearchEngine { get; set; } = string.Empty;

    // [ObservableProperty]
    // public partial bool ShowBackground { get; set; }

    [ObservableProperty]
    public partial int BackgroundBehavior { get; set; }

    [ObservableProperty]
    public partial string BackgroundLocation { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool ShowFlyoutWhenStartDownloading { get; set; }

    [ObservableProperty]
    public partial string SpecificUri { get; set; } = string.Empty;

    [ObservableProperty]
    public partial int StartBehavior { get; set; }

    [ObservableProperty]
    public partial bool DisableGpu { get; set; }

    [ObservableProperty]
    public partial bool DisableBackgroundTimerThrottling { get; set; }

    [ObservableProperty]
    public partial Dictionary<string, bool> ToolBar { get; set; } = new();
}
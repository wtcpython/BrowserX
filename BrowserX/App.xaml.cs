using BrowserX.Controls;
using BrowserX.Helpers;
using BrowserX.Models;
using BrowserX.Pages;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrowserX;

public partial class App : Application
{
    private MainWindow? _window;
    public static Profile Profile;
    public static WebView2 WebView2;
    public static CoreWebView2 CoreWebView2;
    public static CoreWebView2Environment CoreWebView2Environment;
    public static CoreWebView2Profile CoreWebView2Profile;
    public static ObservableCollection<WebViewHistory> Histories = [];
    public static ObservableCollection<DownloadObject> DownloadList = [];
    public static bool NeedRestartEnvironment;
    public static ReleaseWindow? ReleaseWindow { get; set; }

    public App()
    {
        InitializeComponent();
        Profile = ProfileHelper.LoadProfile();
    }

    public async void EnsureWebView2Async()
    {
        List<string> additionalBrowserArguments = [];
        if (Profile.DisableGpu) additionalBrowserArguments.Add("--disable-gpu");
        if (Profile.DisableBackgroundTimerThrottling)
            additionalBrowserArguments.Add("--disable-background-timer-throttling");
        CoreWebView2Environment = await CoreWebView2Environment.CreateWithOptionsAsync(
            null,
            null,
            new CoreWebView2EnvironmentOptions
            {
                AdditionalBrowserArguments = string.Join(" ", additionalBrowserArguments)
            });
        CoreWebView2Environment.BrowserProcessExited += BrowserProcessExited;
        WebView2 = new WebView2();
        await WebView2.EnsureCoreWebView2Async(CoreWebView2Environment);
        CoreWebView2 = WebView2.CoreWebView2;
        CoreWebView2Profile = CoreWebView2.Profile;
    }

    private void BrowserProcessExited(CoreWebView2Environment sender, CoreWebView2BrowserProcessExitedEventArgs args)
    {
        if (NeedRestartEnvironment)
        {
            NeedRestartEnvironment = false;
            EnsureWebView2Async();
        }
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        EnsureWebView2Async();
        _window = WindowHelper.CreateWindow();
        var arguments = Environment.GetCommandLineArgs()[1..];
        if (arguments.Length > 0)
        {
            foreach (var arg in arguments)
                Utilities.Search(arg, _window);
        }
        else
            _window.AddHomePage();

        _window.Activate();

        var notificationManager = AppNotificationManager.Default;
        notificationManager.NotificationInvoked += NotificationManager_NotificationInvoked;
        notificationManager.Register();

        var activatedArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        var activationKind = activatedArgs.Kind;
        if (activationKind != ExtendedActivationKind.AppNotification)
        {
            var presenter = _window.AppWindow.Presenter as OverlappedPresenter;
            presenter?.Restore(true);
        }
        else
            HandleNotification((AppNotificationActivatedEventArgs)activatedArgs.Data);
    }

    private void HandleNotification(AppNotificationActivatedEventArgs args)
    {
        var dispatcherQueue = _window?.DispatcherQueue ?? DispatcherQueue.GetForCurrentThread();

        dispatcherQueue.TryEnqueue(async delegate
        {
            switch (args.Arguments["Notification"])
            {
                case "LaunchReleaseWebsite":
                    await Windows.System.Launcher.LaunchUriAsync(
                        new Uri("https://github.com/wtcpython/WinUIEdge/releases/latest/"));
                    break;
                case "ChangeStartUri":
                    _window?.Content.Focus(FocusState.Programmatic);
                    break;
            }
        });
    }

    private void NotificationManager_NotificationInvoked(AppNotificationManager sender,
        AppNotificationActivatedEventArgs args)
    {
        HandleNotification(args);
    }
}
using BrowserX.Pages;
using CommunityToolkit.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using BrowserX.Helpers;

namespace BrowserX.Controls;

public class WebViewHistory
{
    public string DocumentTitle { get; set; }
    public string Source { get; set; }
    public Uri FaviconUri { get; set; }
    public string Time { get; set; }
    public ulong NavigationId { get; set; }
}

public partial class DownloadObject : INotifyPropertyChanged
{
    public CoreWebView2DownloadOperation Operation { get; set; }
    public string Title { get; set; }

    public double BytesReceived
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BytesReceived)));
        }
    }

    public double TotalBytes { get; set; }

    public string Information
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Information)));
        }
    }

    public DateTime DateTime { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public DownloadObject(CoreWebView2DownloadOperation operation)
    {
        Operation = operation;
        Title = Path.GetFileName(Operation.ResultFilePath);
        TotalBytes = Operation.TotalBytesToReceive;
        DateTime = DateTime.Now;
        Operation.BytesReceivedChanged += Operation_BytesReceivedChanged;
    }

    private void Operation_BytesReceivedChanged(CoreWebView2DownloadOperation sender, object args)
    {
        var receivedDelta =
            Converters.ToFileSizeString((long)((sender.BytesReceived - BytesReceived) /
                                               (DateTime.Now - DateTime).TotalSeconds));
        var received = Converters.ToFileSizeString(sender.BytesReceived);
        var total = Converters.ToFileSizeString(sender.TotalBytesToReceive);
        var speed = receivedDelta + "/s";
        var information =
            $"{speed} - {received}/{total}，剩余时间：{DateTime.Parse(sender.EstimatedEndTime) - DateTime.Now:hh\\:mm\\:ss}";
        BytesReceived = sender.BytesReceived;
        DateTime = DateTime.Now;
        Information = information;
    }
}

public sealed partial class ToolBar : UserControl
{
    public ToolBar()
    {
        InitializeComponent();
        historyList.ItemsSource = App.Histories;
        downloadList.ItemsSource = App.DownloadList;
        FavoriteList.SetItemsPanel(FavoriteList.VerticalTemplate);
        HistoryButton.Visibility = App.Profile.ToolBar!["HistoryButton"] ? Visibility.Visible : Visibility.Collapsed;
        DownloadButton.Visibility = App.Profile.ToolBar!["DownloadButton"] ? Visibility.Visible : Visibility.Collapsed;
    }

    private void SearchHistory(object sender, TextChangedEventArgs e)
    {
        var text = (sender as TextBox)!.Text;
        historyList.ItemsSource = App.Histories
            .Where(x => x.DocumentTitle.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private void OpenHistory(object sender, ItemClickEventArgs e)
    {
        var mainWindow = WindowHelper.GetWindowForElement(this);

        mainWindow!.AddNewTab(new WebViewPage(new Uri((e.ClickedItem as WebViewHistory)!.Source)));
    }

    private void RemoveDownloadItem(object sender, RoutedEventArgs e)
    {
        var deleteObject = (sender as Button)!.DataContext as DownloadObject;
        deleteObject?.Operation.Cancel();
        App.DownloadList.Remove(deleteObject!);
    }

    private void SearchDownload(object sender, TextChangedEventArgs e)
    {
        var text = (sender as TextBox)!.Text;
        historyList.ItemsSource =
            App.DownloadList.Where(x => x.Title.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void ShowFlyout(string name)
    {
        switch (name)
        {
            case "����":
                DownloadButton.ShowFlyout();
                break;
            case "��ʷ��¼":
                HistoryButton.ShowFlyout();
                break;
            case "�ղؼ�":
                FavoriteButton.ShowFlyout();
                break;
        }
    }

    private void SplitWindow(object sender, RoutedEventArgs e)
    {
        var page = WindowHelper.GetWindowForElement(this)?.SelectedItem as WebViewPage;
        page?.CreateSplitWindow();
    }
}
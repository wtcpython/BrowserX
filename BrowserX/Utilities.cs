using BrowserX.Extensions;
using BrowserX.Helpers;
using BrowserX.Models;
using BrowserX.Pages;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BrowserX;

public class Utilities
{
    public static bool OpenInNotepad(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)) return false;

        try
        {
            Process.Start(new ProcessStartInfo("notepad.exe", $"\"{filePath}\"") { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool OpenInPhotos(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath)) return false;

        try
        {
            var uri = "ms-photos:viewer?fileName=" + Uri.EscapeDataString(imagePath);
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<string> GetBingImageUrlAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetStringAsync("https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
        var json = JsonDocument.Parse(response);
        return "https://cn.bing.com" + json.RootElement.GetProperty("images")[0].GetProperty("url").GetString();
    }

    public static void Search(string text, MainWindow mainWindow)
    {
        var uriType = text.DetectUri();
        if (uriType == UriType.WithProtocol)
            Navigate(text, mainWindow);
        else if (uriType == UriType.WithoutProtocol)
            Navigate("https://" + text, mainWindow);
        else if (File.Exists(text))
        {
            var ext = Path.GetExtension(text);
            if (ProfileHelper.LanguageDict.TryGetValue(ext, out _))
                OpenInNotepad(text);
            else if (ProfileHelper.ImageDict.TryGetValue(ext, out _))
                OpenInPhotos(text);
            else
                Navigate(text, mainWindow);
        }
        else
        {
            Navigate(ProfileHelper.SearchEngineList.First(x => x.Name == App.Profile.SearchEngine).Uri + text,
                mainWindow);
        }
    }

    public static void Navigate(string site, MainWindow mainWindow)
    {
        Uri uri = new(site);
        if (mainWindow.TabView.SelectedItem != null && mainWindow.SelectedItem is WebViewPage webviewPage)
            webviewPage.WebView2.Source = uri;
        else
            mainWindow.AddNewTab(new WebViewPage(uri));
    }
}
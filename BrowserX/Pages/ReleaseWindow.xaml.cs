using BrowserX.Extensions;
using Microsoft.UI.Xaml;

namespace BrowserX.Pages;

public sealed partial class ReleaseWindow : Window
{
    public ReleaseWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon("./Assets/icon.ico");
        this.SetBackdrop();
        this.SetThemeColor();

        Closed += (_, _) =>
        {
            if (App.ReleaseWindow == this)
                App.ReleaseWindow = null;
        };
    }
}
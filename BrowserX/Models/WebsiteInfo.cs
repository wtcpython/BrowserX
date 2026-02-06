using System;

namespace BrowserX.Models;

public class WebsiteInfo
{
    public string Name { get; set; }
    public bool CustomIcon { get; set; }
    public Uri Icon { get; set; }
    public Uri Uri { get; set; }
}
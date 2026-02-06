using System;
using System.Text.RegularExpressions;
using BrowserX.Models;

namespace BrowserX.Extensions;

public static class StringExtension
{
    extension(string name)
    {
        public string ToGlyph()
        {
            return name switch
            {
                "back" => "\ue72b",
                "forward" => "\ue72a",
                "reload" => "\ue72c",
                "saveAs" => "\ue792",
                "print" => "\ue749",
                "share" => "\ue72d",
                "emoji" => "\ue899",
                "undo" => "\ue7a7",
                "redo" => "\ue7a6",
                "cut" => "\ue8c6",
                "copy" => "\ue8c8",
                "paste" => "\ue77f",
                "openLinkInNewWindow" => "\ue737",
                "copyLinkLocation" => "\ue71b",
                _ => string.Empty,
            };
        }

        public UriType DetectUri()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return UriType.PlainText;
            }
            if (name.Contains("://"))
            {
                return Uri.IsWellFormedUriString(name, UriKind.Absolute) ? UriType.WithProtocol : UriType.PlainText;
            }

            const string domainPattern = @"^(?!-)[A-Za-z0-9-]+(\.[A-Za-z]{2,})+$";
            if (Regex.IsMatch(name, domainPattern))
            {
                return UriType.WithoutProtocol;
            }
            return UriType.PlainText;
        }
    }
}
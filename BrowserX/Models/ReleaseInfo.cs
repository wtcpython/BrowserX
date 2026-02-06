using System;
using System.Text.Json.Serialization;

namespace BrowserX.Models;

public class ReleaseInfo
{
    [JsonPropertyName("published_at")] public DateTimeOffset PublishedDate { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("tag_name")] public string? TagName { get; set; }

    [JsonPropertyName("body")] public string? ReleaseNotes { get; set; }
}
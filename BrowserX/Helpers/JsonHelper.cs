using BrowserX.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using BrowserX.Models;

namespace BrowserX.Helpers;

public partial class JsonHelper
{
    [JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
    [JsonSerializable(typeof(IList<ReleaseInfo>))]
    [JsonSerializable(typeof(Profile))]
    [JsonSerializable(typeof(ObservableCollection<WebsiteInfo>))]
    [JsonSerializable(typeof(List<WebsiteInfo>))]
    [JsonSerializable(typeof(Dictionary<string, string>))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
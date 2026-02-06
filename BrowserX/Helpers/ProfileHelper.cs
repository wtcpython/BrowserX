using BrowserX.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace BrowserX.Helpers;

public class ProfileHelper
{
    public static Dictionary<string, string> LanguageDict = JsonSerializer.Deserialize(
        File.ReadAllText("./Assets/Data/LanguageType.json"), JsonHelper.JsonContext.Default.DictionaryStringString)!;

    public static Dictionary<string, string> ImageDict =
        JsonSerializer.Deserialize(File.ReadAllText("./Assets/Data/ImageType.json"),
            JsonHelper.JsonContext.Default.DictionaryStringString)!;

    public static List<WebsiteInfo> SearchEngineList =
        JsonSerializer.Deserialize(File.ReadAllText("./Assets/Data/SearchEngine.json"),
            JsonHelper.JsonContext.Default.ListWebsiteInfo)!;

    public static ObservableCollection<WebsiteInfo> Favorites = JsonSerializer.Deserialize(
        File.ReadAllText("./Assets/Data/Favorites.json"),
        JsonHelper.JsonContext.Default.ObservableCollectionWebsiteInfo)!;

    public static Profile LoadProfile(bool overwrite = false)
    {
        const string profileFile = "./profile.json";
        const string defaultProfilePath = "./Assets/Data/DefaultProfile.json";

        if (!File.Exists(profileFile) || overwrite) File.Copy(defaultProfilePath, profileFile, overwrite);
        var profile =
            JsonSerializer.Deserialize(File.ReadAllText(profileFile), JsonHelper.JsonContext.Default.Profile)!;
        return profile;
    }
}
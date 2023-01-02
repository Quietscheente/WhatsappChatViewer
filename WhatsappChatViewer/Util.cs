using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer;

public class Util
{
    private static string appDataDir = FileSystem.AppDataDirectory;

    private static string BaseDataDir()
    {
        return Path.Combine(appDataDir, "WhatsappChatViewer");
    }

    public static string ChatDataDir()
    {
        return Path.Combine(BaseDataDir(), "Chats");
    }

    public static string MetadataDataDir()
    {
        return Path.Combine(BaseDataDir(), "Metadata");
    }

    public static object GetResource(string key)
    {
        return Application.Current!.Resources.MergedDictionaries.Single(d => d.TryGetValue(key, out var _))[key];
    }
}
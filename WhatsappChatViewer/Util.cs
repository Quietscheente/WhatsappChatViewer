using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer;

public class Util
{
    public static object GetResource(string key)
    {
        return Application.Current!.Resources.MergedDictionaries.Single(d => d.TryGetValue(key, out var _))[key];
    }
}
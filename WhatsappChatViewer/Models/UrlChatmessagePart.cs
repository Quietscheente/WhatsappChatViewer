using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models;

public class UrlChatmessagePart : ChatmessagePart
{
    public Command OpenUrlCommand { get; }
    public UrlChatmessagePart(string url)
    {
        Url = url;

        OpenUrlCommand = new(OpenUrl);
    }

    public string Url { get; }

    private async void OpenUrl(object _)
    {
        try
        {
            Uri uri = new (Url);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            
        }
    }
}

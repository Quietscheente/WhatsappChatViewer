using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Services;

namespace WhatsappChatViewer.Models.MessageParts;

public class UrlChatmessagePart : ChatmessagePart
{
    private readonly UiMessageLogger uiMessageLogger;

    public Command OpenUrlCommand { get; }
    public UrlChatmessagePart(string url, UiMessageLogger uiMessageLogger)
    {
        Url = url;
        this.uiMessageLogger = uiMessageLogger;
        OpenUrlCommand = new(OpenUrl);
    }

    public string Url { get; }

    private async void OpenUrl(object _)
    {
        try
        {
            Uri uri = new(Url);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            uiMessageLogger.ShowMessage(ex.Message, UiMessageType.Error);
        }
    }
}

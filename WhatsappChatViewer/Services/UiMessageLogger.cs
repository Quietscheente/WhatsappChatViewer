using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.Services;

public enum UiMessageType
{
    Info,
    Error
}

public class UiMessageLogger
{
    private readonly Lazy<MainPage> mainPage;

    public UiMessageLogger(Lazy<MainPage> mainPage)
    {
        this.mainPage = mainPage;
    }

    public async void ShowMessage(string text, UiMessageType messageType)
    {
        Color background = (Color)Util.GetResource("BackgroundPrimary");
        Color textColor = (Color)Util.GetResource("Secondary");
        Color errorColor = (Color)Util.GetResource("Error");

        CommunityToolkit.Maui.Core.SnackbarOptions options = new()
        {
            BackgroundColor = background,
            TextColor = messageType == UiMessageType.Info ? textColor : errorColor
            
        };

        await Task.Delay(500);
        await mainPage.Value.DisplaySnackbar(text, duration: TimeSpan.FromSeconds(5), visualOptions: options);
    }
}

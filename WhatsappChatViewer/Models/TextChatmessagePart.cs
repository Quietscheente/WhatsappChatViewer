using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models;

public class TextChatmessagePart : ChatmessagePart
{
    public TextChatmessagePart(string text)
    {
        Text = text;
    }

    public string Text { get; }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models;

public class Chatmessage
{
    public DateTime DateTime { get; }
    public string? From { get; }

    public Chatmessage(DateTime dateTime, string? from)
    {
        DateTime = dateTime;
        From = from;
    }

    public List<ChatmessagePart> Parts { get; } = new();
}

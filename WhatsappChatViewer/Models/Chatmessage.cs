using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models;

public class Chatmessage
{
    public string Date { get; }
    public string Time { get; }
    public string? From { get; }

    public Chatmessage(string date, string time, string? from)
    {
        Date = date;
        Time = time;
        From = from;
    }

    public List<ChatmessagePart> Parts { get; } = new();
}

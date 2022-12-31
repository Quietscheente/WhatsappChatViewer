using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models;

public class RawMessage
{
    public RawMessage(DateTime date, string? from, List<string> lines)
    {
        DateTime = date;
        From = from;
        Lines = lines;
    }

    public DateTime DateTime { get; }
    public string? From { get; }
    public List<string> Lines { get; } = new();
}

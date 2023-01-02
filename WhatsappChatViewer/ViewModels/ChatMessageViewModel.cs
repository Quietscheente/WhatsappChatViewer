using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.ViewModels;

public class ChatMessageViewModel
{
    private readonly string? iAmName;

    public ChatMessageViewModel(Chatmessage chatmessage, string? iAmName)
	{
        Chatmessage = chatmessage;
        this.iAmName = iAmName;
    }

    public Chatmessage Chatmessage { get; }
    public bool IsOwn => !string.IsNullOrEmpty(iAmName) && Chatmessage.From == iAmName;
}

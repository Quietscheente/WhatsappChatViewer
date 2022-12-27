using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.ViewModels;

public class ChatMessageViewModel
{

    public ChatMessageViewModel(Chatmessage chatmessage)
	{
        Chatmessage = chatmessage;

    }

    public Chatmessage Chatmessage { get; }
    public bool IsOwn => Chatmessage.From == "Elisabeth";
}

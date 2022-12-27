using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatsappChatViewer.Models;
using WhatsappChatViewer.Services;

namespace WhatsappChatViewer.ViewModels;

public class ChatViewModel
{
    private IEnumerator<Chatmessage>? messageEnumerator = null;
    private readonly ObservableCollection<ChatMessageViewModel> _chatMessages = new();
    private readonly ChatImporter chatImporter;

    private readonly Chat chat;
    public ObservableCollection<ChatMessageViewModel> ChatMessages
    { 
        get 
        {
            LoadMoreMessages(null!);
            return _chatMessages;
        }
    }

    public ICommand DeleteChatCommand { get; }

    public string Name => chat.Name;

    public ChatViewModel(Chat chat, ChatImporter chatImporter)
    {
        
        LoadMoreMessagesCommand = new Command(LoadMoreMessages);
        DeleteChatCommand= new Command(DeleteChat);

        messageEnumerator = chat.Messages().GetEnumerator();
        this.chat = chat;
        this.chatImporter = chatImporter;
    }

    private void DeleteChat(object _)
    {
        chatImporter.DeleteChat(chat);
    }

    private void LoadMoreMessages(object _)
    {
        if (messageEnumerator is null)
            return;

        int amount = _chatMessages.Any() ? 5 : 35;
        int i = 0;
        while (i++ < amount && messageEnumerator.MoveNext())
        {
            _chatMessages.Add(new(messageEnumerator.Current));
        }
    }

    public ICommand LoadMoreMessagesCommand { get; }
}

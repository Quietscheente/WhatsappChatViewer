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
    private readonly ChatsHandler chatsHandler;
    private readonly IAmSelector iAmSelector;
    private readonly UiMessageLogger uiMessageLogger;
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
    public string? IAmName => chat.IAmName;

    public ChatViewModel(Chat chat, ChatsHandler chatsHandler, IAmSelector iAmSelector, UiMessageLogger uiMessageLogger)
    {
        
        LoadMoreMessagesCommand = new Command(LoadMoreMessages);
        DeleteChatCommand= new Command(DeleteChat);

        messageEnumerator = chat.Messages().GetEnumerator();
        this.chat = chat;
        this.chatsHandler = chatsHandler;
        this.iAmSelector = iAmSelector;
        this.uiMessageLogger = uiMessageLogger;
    }

    private void DeleteChat(object _)
    {
        try
        {
            chatsHandler.DeleteChat(chat);
        }
        catch (Exception ex)
        {
            uiMessageLogger.ShowMessage(ex.Message, UiMessageType.Error);
        }
    }

    public async Task SelectIAmName()
    {
        try
        {
            await chat.SelectIAmName(iAmSelector);
        }
        catch (Exception ex)
        {
            uiMessageLogger.ShowMessage(ex.Message, UiMessageType.Error);
        }
    }

    private void LoadMoreMessages(object _)
    {
        if (messageEnumerator is null)
            return;

        int amount = _chatMessages.Any() ? 5 : 35;
        int i = 0;
        while (i++ < amount && messageEnumerator.MoveNext())
        {
            _chatMessages.Add(new(messageEnumerator.Current, chat.IAmName));
        }
    }

    public ICommand LoadMoreMessagesCommand { get; }
}

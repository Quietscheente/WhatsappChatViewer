using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WhatsappChatViewer.Models;
using WhatsappChatViewer.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using Microsoft.Maui.ApplicationModel;
using System.ComponentModel;

namespace WhatsappChatViewer.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly ChatImporter chatImporter;
    private readonly UiMessageLogger messageLogger;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<ChatViewModel> ChatViewModels { get; } = new();

    public ICommand ImportChatCommand { get; }
    public ICommand ChatSelectedCommand { get; }
    public ICommand UnselectChatCommand { get; }

    public bool IsImportingChat { get; private set; } = false;

    public ChatViewModel? SelectedChatViewModel { get; private set; } = null;

    public MainPageViewModel(ChatImporter chatImporter, UiMessageLogger messageLogger)
    {
        ImportChatCommand = new Command(async () => await ImportChatAsync(), () => !IsImportingChat);
        ChatSelectedCommand = new Command(ChatSelected);
        UnselectChatCommand = new Command(UnselectChat);

        this.chatImporter = chatImporter;
        this.messageLogger = messageLogger;

        chatImporter.ChatlistChanged += ChatImporter_ChatlistChanged;

        UpdateChatList();
    }

    private void ChatImporter_ChatlistChanged(object? sender, EventArgs e)
    {
        UpdateChatList();
    }

    private void UnselectChat(object _)
    {
        SelectedChatViewModel = null;
        PropertyChanged?.Invoke(this, new(nameof(SelectedChatViewModel)));
    }

    private void ChatSelected(object obj)
    {
        SelectedChatViewModel = (ChatViewModel)obj;
        PropertyChanged?.Invoke(this, new(nameof(SelectedChatViewModel)));
    }

    private async Task ImportChatAsync()
    {
        var result = await FilePicker.PickAsync();

        if (result != null)
        {
            if (chatImporter.IsChatImported(result.FullPath))
            {
                messageLogger.ShowMessage("Chat already exists.", UiMessageType.Error);
                return;
            }

            try
            {
                IsImportingChat = true;
                PropertyChanged?.Invoke(this, new(nameof(IsImportingChat)));

                messageLogger.ShowMessage($"Importing {result.FileName}...", UiMessageType.Info);

                await chatImporter.ImportAsync(result.FullPath);
            }
            catch (Exception ex)
            {
                messageLogger.ShowMessage(ex.Message, UiMessageType.Error);
            }
            finally
            {
                IsImportingChat = false;
                PropertyChanged?.Invoke(this, new(nameof(IsImportingChat)));
            }
        }
    }

    private void UpdateChatList()
    {
        IEnumerable<Chat> chats = chatImporter.GetChatList();

        ChatViewModels.Clear();
        chats.ToList().ForEach(chat => ChatViewModels.Add(new(chat, chatImporter)));
    }
}

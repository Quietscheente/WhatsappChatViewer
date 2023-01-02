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
    private readonly ChatsHandler chatsHandler;
    private readonly UiMessageLogger uiMessageLogger;
    private readonly IAmSelector iAmSelector;
    private readonly ChatMetadataHandler metadataHandler;
    private ObservableCollection<ChatViewModel>? _chatViewModels;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<ChatViewModel> ChatViewModels
    {
        get
        {
            if (_chatViewModels is null)
            {
                CreateChatViewModels();
                chatsHandler.ChatList.CollectionChanged += ChatList_CollectionChanged;
            }

            return _chatViewModels!;
        }
    }

    private void ChatList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        CreateChatViewModels();
    }

    public ICommand ImportChatCommand { get; }
    public ICommand SelectChatCommand { get; }
    public ICommand UnselectChatCommand { get; }

    public bool IsImportingChat { get; private set; } = false;

    public ChatViewModel? SelectedChatViewModel { get; private set; } = null;

    public MainPageViewModel(ChatsHandler chatsHandler, UiMessageLogger uiMessageLogger, IAmSelector iAmSelector, ChatMetadataHandler metadataHandler)
    {
        ImportChatCommand = new Command(async () => await ImportChatAsync(), () => !IsImportingChat);
        SelectChatCommand = new Command(async (object obj) => await SelectChat(obj));
        UnselectChatCommand = new Command(UnselectChat);

        this.chatsHandler = chatsHandler;
        this.uiMessageLogger = uiMessageLogger;
        this.iAmSelector = iAmSelector;
        this.metadataHandler = metadataHandler;
    }

    private void UnselectChat(object _)
    {
        SelectedChatViewModel = null;
        PropertyChanged?.Invoke(this, new(nameof(SelectedChatViewModel)));
    }

    private async Task SelectChat(object obj)
    {
        var chatViewModel = (ChatViewModel)obj;

        if (chatViewModel.IAmName is null)
            await chatViewModel.SelectIAmName();
            
        SelectedChatViewModel = chatViewModel;
        PropertyChanged?.Invoke(this, new(nameof(SelectedChatViewModel)));
    }

    private void CreateChatViewModels()
    {
        _chatViewModels ??= new();
        _chatViewModels.Clear();

        foreach (Chat chat in chatsHandler.ChatList)
        {
            _chatViewModels.Add(new(chat, chatsHandler, iAmSelector, uiMessageLogger));
        }
    }

    private async Task ImportChatAsync()
    {
        var result = await FilePicker.PickAsync();

        if (result != null)
        {
            if (metadataHandler.MetadataList.Any(meta => meta.Name == ChatsHandler.ChatNameFromZip(result.FullPath)))
            {
                uiMessageLogger.ShowMessage("Chat already exists.", UiMessageType.Error, 1000);
                return;
            }

            IsImportingChat = true;
            PropertyChanged?.Invoke(this, new(nameof(IsImportingChat)));

            uiMessageLogger.ShowMessage($"Importing {result.FileName}...", UiMessageType.Info, 1000);

            try
            {
                await chatsHandler.ImportAsync(result.FullPath);
            }
            catch (ExtractZipException ex)
            {
                uiMessageLogger.ShowMessage("Error extracting zip file\n" + ex.Message, UiMessageType.Error);
            }
            catch (WriteMetadataException ex)
            {
                uiMessageLogger.ShowMessage("Error writing metadata\n" + ex.Message, UiMessageType.Error);
            }
            catch (SerializeMetadataException ex)
            {
                uiMessageLogger.ShowMessage("Error serializing metadata\n" + ex.Message, UiMessageType.Error);
            }
            catch (Exception ex)
            {
                uiMessageLogger.ShowMessage(ex.Message, UiMessageType.Error);
            }
            finally
            {
                IsImportingChat = false;
                PropertyChanged?.Invoke(this, new(nameof(IsImportingChat)));
            }
        }
    }
}

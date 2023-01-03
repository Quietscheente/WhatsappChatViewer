using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;
using static WhatsappChatViewer.MauiProgram;

namespace WhatsappChatViewer.Services;

public class ExtractZipException : Exception { public ExtractZipException(string message) : base(message) { } }

public class ChatsHandler
{
    private readonly UiMessageLogger uiMessageLogger;
    private readonly ChatMetadataHandler chatMetadataHandler;
    private readonly RawMessageReaderFactory rawMessageReaderFactory;
    private ObservableCollection<Chat>? _chatList;

    public ObservableCollection<Chat> ChatList
    {
        get
        {
            if (_chatList is null)
                CreateChatList();

            return _chatList!;
        }
    }

    public async Task ImportAsync(string zipFilePath)
    {
        string chatName = ChatNameFromZip(zipFilePath);
        string chatPath = GetChatDataDir(zipFilePath);

        ChatMetadata meta = new(chatName, chatPath, chatMetadataHandler);
        chatMetadataHandler.MetadataList.Add(meta);
        chatMetadataHandler.Save();

        try
        {
            await ExtractAsync(zipFilePath, chatPath);
        }
        catch (Exception ex)
        {
            throw new ExtractZipException(ex.Message);
        }

        RawMessageReader messageReader = rawMessageReaderFactory(ChatFilePath(chatPath));
        ChatList.Add(new(meta, messageReader, uiMessageLogger));
    }

    private static async Task ExtractAsync(string zipFilePath, string destPath)
    {
        try
        {
            await ExtractZipAsync(zipFilePath, destPath);
        }
        catch
        {
            if (Directory.Exists(destPath))
                Directory.Delete(destPath, true);

            throw;
        }
        finally
        {
            try
            {
                File.Delete(zipFilePath);
            }
            catch
            { }
        }
    }

    public void DeleteChat(Chat chat)
    {
        var metaToRemove = chatMetadataHandler.MetadataList.Single(meta => meta.Name == chat.Name);

        Directory.Delete(metaToRemove.Directory, true);
        ChatList.Remove(chat);

        chatMetadataHandler.MetadataList.Remove(metaToRemove);
        chatMetadataHandler.Save();  
    }

    private void CreateChatList()
    {
        _chatList = new();

        foreach (var meta in chatMetadataHandler.MetadataList)
        {
            RawMessageReader messageReader = rawMessageReaderFactory(ChatFilePath(meta.Directory));
            _chatList.Add(new Chat(meta, messageReader, uiMessageLogger));
        }
    }

    public ChatsHandler(UiMessageLogger uiMessageLogger, ChatMetadataHandler chatMetadataHandler, RawMessageReaderFactory rawMessageReaderFactory)
    {
        this.uiMessageLogger = uiMessageLogger;
        this.chatMetadataHandler = chatMetadataHandler;
        this.rawMessageReaderFactory = rawMessageReaderFactory;
    }

    public static string ChatFilePath(string chatPath) => Path.Combine(chatPath, "_chat.txt");

    private static async Task ExtractZipAsync(string zipFilePath, string destPath)
    {
        await Task.Run(() =>
        {
            ZipFile.ExtractToDirectory(zipFilePath, destPath);
        });
    }

    public static string ChatNameFromZip(string zipFilePath)
    {
        string name = Path.GetFileNameWithoutExtension(zipFilePath);
        return name.Replace("WhatsApp Chat - ", "");
    }

    private static string GetChatDataDir(string zipFilePath)
    {
        return Path.Combine(Util.ChatDataDir(), ChatNameFromZip(zipFilePath));
    }
}

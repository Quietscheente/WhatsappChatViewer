using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.Services;

public class ChatImporter
{
    private readonly string appDataDir = FileSystem.AppDataDirectory;
    private readonly string baseChatDir;

    public event EventHandler? ChatlistChanged;

    public ChatImporter()
    {
        baseChatDir = Path.Combine(appDataDir, "WhatsappChatViewer");
    }

    public async Task<string> ImportAsync(string zipFilePath)
    {
        string cacheDir = FileSystem.Current.CacheDirectory;
        Directory.GetDirectories(cacheDir).ToList()
            .ForEach(d => Directory.Delete(d, true));

        try
        {
            return await ExtractZipAsync(zipFilePath);
        }
        catch
        {
            if (Directory.Exists(GetChatDataDir(zipFilePath)))
                Directory.Delete(GetChatDataDir(zipFilePath), true);

            throw;
        }
        finally
        {
            ChatlistChanged?.Invoke(this, EventArgs.Empty);

            Directory.GetDirectories(cacheDir).ToList()
                .ForEach(d => Directory.Delete(d, true));
        }
    }

    public bool IsChatImported(string zipFilePath)
    {
        string extractedChatDir = GetChatDataDir(zipFilePath);
        return Directory.Exists(extractedChatDir);
    }

    internal void DeleteChat(Chat chat)
    {
        string dir = chat.Directory;
        Directory.Delete(dir, true);
        ChatlistChanged?.Invoke(this, EventArgs.Empty);
    }

    internal IEnumerable<Chat> GetChatList()
    {
        if (!Directory.Exists(baseChatDir))
            yield break;

        foreach (var chatDir in Directory.GetDirectories(baseChatDir))
        {
            string chatName = new DirectoryInfo(chatDir).Name;
            chatName = chatName.Replace("WhatsApp Chat - ", "");

            yield return new Chat(chatName, chatDir, 
                new MessageSplitter(
                    new Lazy<IEnumerable<string>>(() => File.ReadLines(Path.Combine(chatDir, "_chat.txt")))));
        }
    }

    private async Task<string> ExtractZipAsync(string zipFilePath)
    {
        string extractedChatDir = GetChatDataDir(zipFilePath);

        await Task.Run(() =>
        {
            //await Task.Delay(2000);
            ZipFile.ExtractToDirectory(zipFilePath, extractedChatDir);
        });

        return extractedChatDir;
    }

    private string GetChatDataDir(string zipFilePath)
    {
        string chatName = Path.GetFileNameWithoutExtension(zipFilePath);
        return Path.Combine(baseChatDir, chatName);
    }
}

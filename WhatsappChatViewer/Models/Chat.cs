using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WhatsappChatViewer.Services;

namespace WhatsappChatViewer.Models;

public partial class Chat
{
    private readonly ChatMetadata chatMetadata;
    private readonly RawMessageReader rawMessageReader;
    private readonly UiMessageLogger uiMessageLogger;

    public string Name => chatMetadata.Name;
    public string? IAmName => chatMetadata.IAmName;

    public Chat(ChatMetadata chatMetadata, RawMessageReader rawMessageReader, UiMessageLogger uiMessageLogger)
    {
        this.chatMetadata = chatMetadata;
        this.rawMessageReader = rawMessageReader;
        this.uiMessageLogger = uiMessageLogger;
    }

    public IEnumerable<string> Authors() => rawMessageReader.GetRawMessages().Where(msg => msg.From is not null).Select(msg => msg.From!).Distinct().Order();

    public async Task SelectIAmName(IAmSelector iAmSelector)
    {
        var authors = Authors();
        string? iAmName = await iAmSelector.GetIAmName(authors);
        iAmName ??= string.Empty;

        chatMetadata.IAmName = iAmName;
        chatMetadata.ChatMetadataHandler.Save();
    }

    public IEnumerable<Chatmessage> Messages()
    {
        foreach (var rawMessage in rawMessageReader.GetRawMessages().Reverse())
        {
            var chatMessage = new Chatmessage(rawMessage.DateTime, rawMessage.From);

            foreach (string rawMessageLine in rawMessage.Lines)
            {
                bool isImage = false;

                var matchAttachment = Regex.Match(rawMessageLine, @"<(?:.+?): (.+)>");
                if (matchAttachment.Success)
                {
                    string filename = matchAttachment.Groups[1].Value;
                    if (Path.GetExtension(filename) is ".jpg" or ".webp" && File.Exists(Path.Combine(chatMetadata.Directory, filename)))
                    {
                        isImage = true;
                        ImageSource imageSource = ImageSource.FromFile(Path.Combine(chatMetadata.Directory, filename));
                        chatMessage.Parts.Add(new ImageChatmessagePart(imageSource));
                    }
                }

                if (!isImage)
                {
                    var urlPattern = IsUrlRegex();
                    var chunks = urlPattern.Split(rawMessageLine);

                    for (int i = 0; i < chunks.Length; i++)
                    {
                        if (chunks[i].Trim().Length == 0)
                            continue;

                        if (i % 2 == 0)
                            chatMessage.Parts.Add(new TextChatmessagePart(chunks[i].Trim()));
                        else
                            chatMessage.Parts.Add(new UrlChatmessagePart(chunks[i].Trim(), uiMessageLogger));
                    }
                }
            }

            yield return chatMessage;
        }
    }

    [GeneratedRegex("((?:(?:https?|ftp|file):\\/\\/|www\\.|ftp\\.)(?:\\([-A-Z0-9+&@#\\/%=~_|$?!:,.]*\\)|[-A-Z0-9+&@#\\/%=~_|$?!:,.])*(?:\\([-A-Z0-9+&@#\\/%=~_|$?!:,.]*\\)|[A-Z0-9+&@#\\/%=~_|$]))", RegexOptions.IgnoreCase)]
    private static partial Regex IsUrlRegex();
}

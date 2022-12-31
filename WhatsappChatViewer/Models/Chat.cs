using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WhatsappChatViewer.Services;
using static System.Net.Mime.MediaTypeNames;

namespace WhatsappChatViewer.Models;

public partial class Chat
{
    private readonly MessageSplitter messageSplitter;

    public string Name { get; }
    public string Directory { get; }

    public Chat(string name, string directory, MessageSplitter messageSplitter)
    {
        Name = name;
        Directory = directory;
        this.messageSplitter = messageSplitter;
    }

    public IEnumerable<Chatmessage> Messages()
    {
        foreach (var rawMessage in messageSplitter.GetRawMessages().Reverse())
        {
            var chatMessage = new Chatmessage(rawMessage.DateTime, rawMessage.From);

            foreach (string rawMessageLine in rawMessage.Lines)
            {
                bool isImage = false;

                var matchAttachment = Regex.Match(rawMessageLine, @"<(?:.+?): (.+)>");
                if (matchAttachment.Success)
                {
                    string filename = matchAttachment.Groups[1].Value;
                    if (Path.GetExtension(filename) is ".jpg" or ".webp" && File.Exists(Path.Combine(Directory, filename)))
                    {
                        isImage = true;
                        ImageSource imageSource = ImageSource.FromFile(Path.Combine(Directory, filename));
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
                            chatMessage.Parts.Add(new UrlChatmessagePart(chunks[i].Trim()));
                    }
                }
            }

            yield return chatMessage;
        }
    }

    [GeneratedRegex("((?:(?:https?|ftp|file):\\/\\/|www\\.|ftp\\.)(?:\\([-A-Z0-9+&@#\\/%=~_|$?!:,.]*\\)|[-A-Z0-9+&@#\\/%=~_|$?!:,.])*(?:\\([-A-Z0-9+&@#\\/%=~_|$?!:,.]*\\)|[A-Z0-9+&@#\\/%=~_|$]))", RegexOptions.IgnoreCase)]
    private static partial Regex IsUrlRegex();
}

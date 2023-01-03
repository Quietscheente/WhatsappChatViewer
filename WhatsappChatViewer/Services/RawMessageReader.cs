using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.Services;

public partial class RawMessageReader
{
    private readonly string chatFilePath;

    [GeneratedRegex(@"^\u200e?\[(.+?)\](?: (.+?):)? \u200e?(.*)")]
    private static partial Regex IsNewMessageRegex();

    public RawMessageReader(string chatFilePath)
	{
        this.chatFilePath = chatFilePath;
    }

    public IEnumerable<RawMessage> GetRawMessages()
    {
        RawMessage? lastMessage = null;

        foreach (string line in File.ReadLines(chatFilePath))
        {
            bool isNewMessage = false;

            var match = IsNewMessageRegex().Match(line);
            if (match.Success)
            {
                string possbileDateTime = match.Groups[1].Value;
                if (DateTime.TryParse(possbileDateTime, out DateTime dateTime))
                {
                    isNewMessage = true;
                    string from = match.Groups[2].Value;
                    string content = match.Groups[3].Value;

                    if (lastMessage is not null)
                        yield return lastMessage;

                    lastMessage = new RawMessage(dateTime, string.IsNullOrEmpty(from) ? null : from, new() { content });
                }
            }
            else if (!isNewMessage && lastMessage is not null)
            {
                lastMessage.Lines.Add(line);
            }
            else
                throw new Exception("Unknown chat format.");
        }

        if (lastMessage is not null)
            yield return lastMessage;
    }
}


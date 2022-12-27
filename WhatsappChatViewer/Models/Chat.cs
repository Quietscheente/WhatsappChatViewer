﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WhatsappChatViewer.Models;

public class Chat
{
    public string Name { get; }
    public string Directory { get; }

    private readonly Lazy<IEnumerable<string>> lazyMessageLines;

    public IEnumerable<Chatmessage> Messages()
    {
        Chatmessage? lastMessage = null;

        foreach (string line in lazyMessageLines.Value)
        {
            Chatmessage message;
            string content;

            var match = Regex.Match(line, @"\[(\d\d\.\d\d\.\d\d), (\d\d:\d\d:\d\d)\](?: (.+?):)? (.*)");
            if (match.Success)
            {
                string date = match.Groups[1].Value;
                string time = match.Groups[2].Value;
                string from = match.Groups[3].Value;
                content = match.Groups[4].Value;

                message = new(date, time, from);
            }
            else if (lastMessage is not null)
            {
                message = lastMessage;
                content = line;
            }
            else
                throw new Exception("Unknown chat format.");

            ImageSource? imageSource = null;
            var matchAttachment = Regex.Match(content, @"<Anhang: (.+)>");
            if (matchAttachment.Success)
            {
                string filename = matchAttachment.Groups[1].Value;
                if (Path.GetExtension(filename) == ".jpg" && File.Exists(Path.Combine(Directory, filename)))
                {
                    imageSource = ImageSource.FromFile(Path.Combine(Directory, filename));
                    message.Parts.Add(new ImageChatmessagePart(imageSource));
                }
            }

            if (imageSource is null)
                message.Parts.Add(new TextChatmessagePart(content));

            if (message != lastMessage)
            {
                if (lastMessage is not null)
                    yield return lastMessage;

                lastMessage = message;
            }
        }

        if (lastMessage is not null)
            yield return lastMessage;
    }

    public Chat(string name, string directory, Lazy<IEnumerable<string>> messageLines)
	{
        Name = name;
        this.Directory = directory;
        lazyMessageLines = messageLines;
    }
}
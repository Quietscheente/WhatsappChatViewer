using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WhatsappChatViewer.Services;

namespace WhatsappChatViewer.Models;

public class ChatMetadata
{
    [JsonConstructor]
    public ChatMetadata(string? iAmName, string name, string directory)
    {
        IAmName = iAmName;
        Name = name;
        Directory = directory;
        ChatMetadataHandler = null!;
    }

    public ChatMetadata(string name, string directory, ChatMetadataHandler chatMetadataHandler)
    {
        Name = name;
        Directory = directory;
        ChatMetadataHandler = chatMetadataHandler;
    }


    public string? IAmName { get; set; } = null;
    public string Name { get; set; }
    public string Directory { get; }

    [JsonIgnore]
    public ChatMetadataHandler ChatMetadataHandler { get; set; }
}

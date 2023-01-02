using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.Services;

public class ReadMetadataException : Exception { public ReadMetadataException(string message) : base(message) { } }
public class WriteMetadataException : Exception { public WriteMetadataException(string message) : base(message) { } }
public class DeserializeMetadataException : Exception { public DeserializeMetadataException(string message) : base(message) { } }
public class SerializeMetadataException : Exception { public SerializeMetadataException(string message) : base(message) { } }

public class ChatMetadataHandler
{
    private List<ChatMetadata>? _metadataList;

    public List<ChatMetadata> MetadataList
    {
        get
        {
            if (_metadataList is null)
                CreateMetadataList();

            return _metadataList!;
        }
    }

    private void CreateMetadataList()
    {
        _metadataList ??= new();
        _metadataList.Clear();

        string metadataDir = Util.MetadataDataDir();
        string chatMetadataFile = Path.Combine(metadataDir, "metadata.json");

        if (!Directory.Exists(metadataDir) || !File.Exists(chatMetadataFile))
            return;

        string json;
        try
        {
            json = File.ReadAllText(chatMetadataFile);
        }
        catch (Exception ex)
        {
            throw new ReadMetadataException(ex.Message);
        }

        try
        {
            _metadataList = JsonSerializer.Deserialize<List<ChatMetadata>>(json)!;
            foreach (var meta in _metadataList)
                meta.ChatMetadataHandler = this;
        }
        catch (Exception ex)
        {
            throw new DeserializeMetadataException(ex.Message);
        }
    }

    public void Save()
    {
        string metadataDir = Util.MetadataDataDir();
        string chatMetadataFile = Path.Combine(metadataDir, "metadata.json");

        string json;
        try
        {
            json = JsonSerializer.Serialize(_metadataList);
        }
        catch (Exception ex)
        {
            throw new SerializeMetadataException(ex.Message);
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(chatMetadataFile)!);
            File.WriteAllText(chatMetadataFile, json);
        }
        catch (Exception ex)
        {
            throw new WriteMetadataException(ex.Message);
        }
    }
}

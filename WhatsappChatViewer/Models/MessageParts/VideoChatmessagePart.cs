using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models.MessageParts;

public class VideoChatmessagePart : ChatmessagePart, INotifyPropertyChanged
{
    public Command PlayVideoCommand { get; }
    public bool IsLoadingVideoPlayer { get; private set; } = false;
    public bool IsNotLoadingVideoPlayer => !IsLoadingVideoPlayer;

    public VideoChatmessagePart(string filePath)
    {
        FilePath = filePath;

        PlayVideoCommand = new Command(async _ => await PlayVideo());
    }

    public string FilePath { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async Task PlayVideo()
    {
        IsLoadingVideoPlayer = true;
        PropertyChanged?.Invoke(this, new(nameof(IsLoadingVideoPlayer)));
        PropertyChanged?.Invoke(this, new(nameof(IsNotLoadingVideoPlayer)));

        try
        {
            await Launcher.Default.OpenAsync(new OpenFileRequest("Play audio file", new ReadOnlyFile(FilePath)));
        }
        finally
        {
            IsLoadingVideoPlayer = false;
            PropertyChanged?.Invoke(this, new(nameof(IsLoadingVideoPlayer)));
            PropertyChanged?.Invoke(this, new(nameof(IsNotLoadingVideoPlayer)));
        }
    }
}

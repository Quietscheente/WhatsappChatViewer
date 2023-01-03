using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models.MessageParts
{
    internal class AudioChatmessagePart : ChatmessagePart, INotifyPropertyChanged
    {
        public Command PlayAudioCommand { get; }
        public bool IsLoadingAudioPlayer { get; private set; } = false;
        public bool IsNotLoadingAudioPlayer => !IsLoadingAudioPlayer;

        public AudioChatmessagePart(string filePath)
        {
            FilePath = filePath;

            PlayAudioCommand = new Command(async _ => await PlayAudio());
        }

        public string FilePath { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task PlayAudio()
        {
            IsLoadingAudioPlayer = true;
            PropertyChanged?.Invoke(this, new(nameof(IsLoadingAudioPlayer)));
            PropertyChanged?.Invoke(this, new(nameof(IsNotLoadingAudioPlayer)));

            try
            {
                await Launcher.Default.OpenAsync(new OpenFileRequest("Play audio file", new ReadOnlyFile(FilePath)));
            }
            finally
            {
                IsLoadingAudioPlayer = false;
                PropertyChanged?.Invoke(this, new(nameof(IsLoadingAudioPlayer)));
                PropertyChanged?.Invoke(this, new(nameof(IsNotLoadingAudioPlayer)));
            }
        }
    }
}

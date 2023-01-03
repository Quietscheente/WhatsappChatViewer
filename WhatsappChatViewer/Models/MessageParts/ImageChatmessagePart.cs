using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.Models.MessageParts;

public class ImageChatmessagePart : ChatmessagePart
{
    public ImageChatmessagePart(ImageSource imageSource)
    {
        ImageSource = imageSource;
    }

    public ImageSource ImageSource { get; }
}

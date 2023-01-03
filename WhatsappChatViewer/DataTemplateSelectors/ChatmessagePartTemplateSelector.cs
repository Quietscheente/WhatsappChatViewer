using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models.MessageParts;

namespace WhatsappChatViewer.DataTemplateSelectors;

public class ChatmessagePartTemplateSelector : DataTemplateSelector
{
    public DataTemplate TextTemplate { get; set; } = null!;
    public DataTemplate UrlTemplate { get; set; } = null!;
    public DataTemplate ImageTemplate { get; set; } = null!;
    public DataTemplate AudioTemplate { get; set; } = null!;
    public DataTemplate VideoTemplate { get; set; } = null!;

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return item switch
        {
            ImageChatmessagePart => ImageTemplate,
            TextChatmessagePart => TextTemplate,
            UrlChatmessagePart => UrlTemplate,
            AudioChatmessagePart => AudioTemplate,
            VideoChatmessagePart => VideoTemplate,
            _ => throw new NotImplementedException()
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappChatViewer.Models;

namespace WhatsappChatViewer.DataTemplateSelectors;

public class ChatmessagePartTemplateSelector : DataTemplateSelector
{
    public DataTemplate TextTemplate { get; set; } = null!;
    public DataTemplate ImageTemplate { get; set; } = null!;

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return item switch
        {
            ImageChatmessagePart => ImageTemplate,
            TextChatmessagePart => TextTemplate,
            _ => throw new NotImplementedException()
        };
    }
}

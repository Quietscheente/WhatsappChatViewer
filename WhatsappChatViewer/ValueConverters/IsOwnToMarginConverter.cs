using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappChatViewer.ValueConverters;

public class IsOwnToMarginConverter : IValueConverter
{
    public static IsOwnToMarginConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isOwn = (bool)value;

        return isOwn ? new Thickness(50, 2, 2, 2) : new Thickness(2, 2, 50, 2);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

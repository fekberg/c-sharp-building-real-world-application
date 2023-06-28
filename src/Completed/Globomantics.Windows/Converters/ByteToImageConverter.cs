using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Globomantics.Windows.Converters;

public class ByteToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not byte[] data)
        {
            return null!;
        }

        var bitmap = new ImageSourceConverter().ConvertFrom(data) as BitmapSource;

        return bitmap!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

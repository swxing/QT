using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QT.UI.Converters
{
   public class ChangeToBrushConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value == null)
            return Brushes.White;

         if (!double.TryParse(value.ToString(), out var v))
            return Brushes.White;

         if (v > 0)
            return Brushes.Red;      // 漲紅
         if (v < 0)
            return Brushes.LimeGreen; // 跌綠

         return Brushes.White;       // 不漲不跌
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         => throw new NotImplementedException();
   }
}
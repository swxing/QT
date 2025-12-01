using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QT.UI.Views
{
   // 將布林值 (IsPositiveChange) 轉換為顏色
   public class ChangeToColorConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (value is bool isPositive)
         {
            // 圖片中，漲 (IsPositiveChange=true) 是紅色的
            if (isPositive)
            {
               return new SolidColorBrush(Color.FromRgb(240, 80, 70)); // 類似圖片中的紅色
            }
            else
            {
               return new SolidColorBrush(Color.FromRgb(100, 180, 90)); // 綠色
            }
         }
         return Brushes.Black; // 預設顏色
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}

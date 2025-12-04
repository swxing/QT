using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QT.UI.Converters
{
   /// <summary>
   /// 將 float 類型的漲跌數值轉換為顏色 (正紅/負綠)。
   /// </summary>
   public class PriceToColorConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         // 檢查目標類型是否為 Brush
         if (targetType != typeof(Brush))
            return Brushes.Black;

         // 確保輸入值是 float 類型
         if (!(value is float changeValue))
         {
            // 如果不是 float，則嘗試解析，否則返回黑色作為預設值或錯誤指示
            if (value is IConvertible convertible)
            {
               try
               {
                  changeValue = convertible.ToSingle(culture);
               }
               catch
               {
                  return Brushes.Gray; // 無法轉換時，設為持平色
               }
            }
            else
            {
               return Brushes.Gray; // 無法處理的類型，設為持平色
            }
         }

         // 判斷顏色
         // 使用一個微小的容差 (Epsilon) 來處理浮點數精確度問題
         const float Epsilon = 0.0001f;

         if (changeValue > Epsilon)
         {
            // 正值：紅色
            return new SolidColorBrush(Color.FromRgb(240, 80, 70));
         }
         else if (changeValue < -Epsilon)
         {
            // 負值：綠色
            return new SolidColorBrush(Color.FromRgb(100, 180, 90));
         }
         else
         {
            // 接近零或持平：灰色
            return Brushes.Gray;
         }
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         // 此轉換器不支持反向轉換
         throw new NotSupportedException();
      }
   }

}

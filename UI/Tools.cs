using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace QT.UI
{
   internal class Tools
   {
      /// <summary>字型大小為14。</summary>
      /// <param name="msg"></param>
      /// <param name="visual">通常帶this</param>
      /// <returns></returns>
      public static System.Windows.Media.FormattedText GetFormattedText(string msg, Brush brush)
      {
         return GetFormattedText(msg, brush, 14);
      }


      /// <summary>建立白色的文字格式</summary>
      public static System.Windows.Media.FormattedText GetFormattedText(string msg, Brush brush, double fontSize)
      {
         CultureInfo culture = CultureInfo.CurrentCulture;
         var typeFace = new Typeface("Verdana");
         var ft = new FormattedText(msg, culture, FlowDirection.LeftToRight, typeFace, fontSize,
            brush, VisualTreeHelper.GetDpi(System.Windows.Application.Current.MainWindow).PixelsPerDip);
         return ft;
      }
   }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QT.UI
{
   public class Converter
   {   /// <summary>Converter是繫結到物件，再利用ColumnInfo來處理值的問題，
       /// 這樣好處是可擴充。可以處理一個屬性，多種表態的方式。</summary>
      public class ColumnConverter : IValueConverter
      {
         public ColumnConverter(ColumnInfo info)
         {

            this.ColumnInfo = info;
         }
         public ColumnInfo ColumnInfo { get; set; }

         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
            //取得值。我們無知道確切的型別，用propertyInfo來操作。
            var rowDI = value as RowDI;
            if (rowDI == null)
               return value;

            if (rowDI != null && rowDI.IsTranslation == true)
               return rowDI.GetTranslation(this.ColumnInfo.BindingName);

            var pi = value.GetType().GetProperty(ColumnInfo.BindingName);
            var vv = pi.GetValue(value, null);

            if (this.ColumnInfo.DataType == typeof(Decimal))
            {
               var raw = (decimal)vv;
               var money = Decimal.Round(raw, this.ColumnInfo.ZeroDigits);
               if (this.ColumnInfo.IsEmptyWhenZero == true && money == 0)
                  return "";
               return raw.ToString(ColumnInfo.FormatterString);
            }
            else if (this.ColumnInfo.DataType == typeof(float))
            {
               var raw = (float)vv;

               var money = MathF.Round(raw, this.ColumnInfo.ZeroDigits);
               if (this.ColumnInfo.IsEmptyWhenZero == true && money == 0)
                  return "";
               return raw.ToString(ColumnInfo.FormatterString);
            }
            else if (this.ColumnInfo.DataType == typeof(int))
            {
               var raw = (int)vv;
               if (this.ColumnInfo.IsEmptyWhenZero == true && raw == 0)
                  return "";
               return raw.ToString(ColumnInfo.FormatterString);
            }
            else if (this.ColumnInfo.DataType == typeof(long))
            {
               var raw = (long)vv;
               if (this.ColumnInfo.IsEmptyWhenZero == true && raw == 0)
                  return "";
               return raw.ToString(ColumnInfo.FormatterString);
            }
            else if (this.ColumnInfo.DataType == typeof(DateTime))
            {
               var raw = (DateTime)vv;

               return raw.ToString(ColumnInfo.FormatterString);
            }

            else
               return vv;
         }

         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
            throw new NotImplementedException();
         }
      }


      /// <summary>根據IRowDI.DataLayer. 分別會回傳訂義的色彩</summary>
      public class RowDIToBackgroundConverter : IValueConverter
      {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
            if (value == null)
               return null;
            var rowDI = value as RowDI;
            if (rowDI == null)
               return value;


            //@ 如果Background非null時，以background為主。
            if (rowDI.Background != null)
               return rowDI.Background;

            if (rowDI.DataLayer == 0)
               return Brushes.White;
            else if (rowDI.DataLayer == 1)
               return Brushes.Lavender;
            else if (rowDI.DataLayer == 2)
               return Brushes.LightGray;
            else if (rowDI.DataLayer == 3)
               return Brushes.LightCyan;
            else
               return Brushes.Pink;


         }

         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
            throw new NotImplementedException();
         }
      }

      /// <summary>根據IRowDI.DataLayer. FontSize，回傳一個int</summary>
      public class RowDIToFontSizeConverter : IValueConverter
      {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
            if (value == null)
               return null;

            var rowDI = (RowDI)value;

            if (rowDI.DataLayer == 0)
               return 14;
            else if (rowDI.DataLayer == 1)
               return 16;
            else
               return 18;

         }

         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
            throw new NotImplementedException();
         }
      }

      /// <summary>根據IRowDI.DataLayer. 回傳一個FontWeight</summary>
      public class RowDIToFontWeightConverter : IValueConverter
      {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
            if (value == null)
               return null;

            var rowDI = (RowDI)value;

            if (rowDI.DataLayer == 0)
               return FontWeights.Normal;
            else
               return FontWeights.Bold;
            //else if (rowDI.DataLayer == 1)
            //   return FontWeights.Bold;
            //else
            //   return FontWeights.UltraBold;
         }

         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
            throw new NotImplementedException();
         }
      }


      /// <summary>正負數的色彩轉換。</summary>
      public class NavigatorToBrushConverter : IValueConverter
      {

         public NavigatorToBrushConverter(Brush normal, Brush navigator)
         {
            this._normalBrush = normal;
            this._navigatorBrush = navigator;
         }

         private Brush _normalBrush;
         private Brush _navigatorBrush;

         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
         {
            if (value == null)
               return _normalBrush;

            float number;
            if (float.TryParse(value.ToString(), out number))
            {
               return number < 0 ? _navigatorBrush : _normalBrush;
            }
            return _normalBrush;
         }

         public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         {
            throw new NotImplementedException();
         }
      }
   }
}

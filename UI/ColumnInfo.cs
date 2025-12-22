using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QT.UI
{

   /// <summary>
   /// 屬性上的一個 attribute，請記得資料要繼承自合RowDI
   /// </summary>
   public class ColumnInfo
   {

      /// <summary>header等於bindingName</summary>
      /// <param name="headerName"></param>
      public ColumnInfo(string headerName)
      {
         this.HeaderName = headerName;
         this.BindingName = headerName;
      }

      public ColumnInfo(string headerName, Type type, string formatter)
      {
         this.HeaderName = headerName;
         this.BindingName = headerName;
         this.DataType = type;
         this.FormatterString = formatter;
      }

      public ColumnInfo(string headerName, string bindingName)
      {
         this.HeaderName = headerName;
         this.BindingName = bindingName;
      }

      public Brush HeaderForground { get; set; } = Brushes.Black;

      public ColumnInfo(string headerName, string bindingName, Type type, string formatter)
      {
         this.HeaderName = headerName;
         this.BindingName = bindingName;
         this.DataType = type;
         this.FormatterString = formatter;

      }

      /// <summary>呈現的header</summary>
      public string HeaderName { get; set; }

      /// <summary>這個屬性並不會改變數值。僅是在判斷若型別為Decimal時，若IsEmptyWhenZero時會使用Digits來判斷是否為零。</summary>
      public int ZeroDigits { get; set; } = 0;     //位數，

      /// <summary>繫結的屬性名。</summary>
      public string BindingName { get; set; }

      /// <summary>用於將欄位大類，以方便UI來顯現/隱藏分類。</summary>
      public object Classify { get; set; }

      public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

      /// <summary>欄位的Tip。</summary>
      public string Tip { get; set; } = "";

      /// <summary>資料型別，如果為number，則會套用FormatterString, IsEmptyWhenZeor,</summary>
      public Type DataType { get; set; }

      /// <summary>格式化字串，目前只用在Decimal的型別。</summary>
      public string FormatterString { get; set; }

      /// <summary>設定其背景色。目前沒有用。</summary>
      public SolidColorBrush Background { get; set; }


      public Brush Foreground { get; set; } = Brushes.Black;


      /// <summary>當DataType為number才有用。</summary>
      public Brush ForegroundWhenNavigator { get; set; } = Brushes.Red;


      /// <summary>當DataType為Decimal時且值為0，則為空字串，注意是取Digits後的值。</summary>
      public bool IsEmptyWhenZero { get; set; } = false;



      /// <summary>文字是否能折行，這會用到elementStyle</summary>
      public bool IsWrap { get; set; } = false;

      public DataGridLength Width { get; set; } = DataGridLength.Auto;

   }

}

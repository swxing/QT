
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

using static QT.UI.Converter;

namespace QT.UI
{
   public static class DataGridHelper
   {

      /// <summary>若要分行，可使用\r\n。</summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public static System.Windows.Controls.Label CreateDataGridHeader(string name)
      {
         return CreateDataGridHeader(name, Brushes.Black);
      }

      public static System.Windows.Controls.Label CreateDataGridHeader(string name, Brush foreground)
      {
         Label lblAR = new Label();

         lblAR.Content = name;
         lblAR.Foreground = foreground;
         lblAR.FontSize = 18;
         lblAR.FontWeight = FontWeights.Bold;
         lblAR.HorizontalContentAlignment = HorizontalAlignment.Center;
         lblAR.HorizontalAlignment = HorizontalAlignment.Center;
         return lblAR;
      }

      /// <summary>建立一個通用呈現的Column。記住，資料來源要繼承自RowDI</summary>
      /// <param name="info"></param>
      /// <returns></returns>
      public static DataGridColumn CreateDataGridTextColumn(ColumnInfo info)
      {

         DataGridTextColumn column = new DataGridTextColumn();       //先給TextColumn
         var lbl = CreateDataGridHeader(info.HeaderName, info.HeaderForground);
         if (info.Tip != null && info.Tip.Trim() != "")
         {
            //TextBlock teb = new TextBlock();
            //teb.Text = info.Tip;
            lbl.ToolTip = info.Tip;
         }

         ToolTipService.SetInitialShowDelay(lbl, 50);
         column.Header = lbl;

         Binding binding = new Binding();          //繫結整個物件。
         ColumnConverter converter = new ColumnConverter(info);
         binding.Converter = converter;
         column.Binding = binding;
         column.CanUserSort = true;

         column.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
         column.SortMemberPath = binding.ElementName;

         //CellStyle與ElementStyle的不同：ElementStyle在非編輯模式，影響的是TextBlock。
         //CellStyle在編輯模式，影響的是TextBox。

         Style elementStyle = new Style(typeof(TextBlock));

         elementStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, info.TextAlignment));

         Binding bindingFG = new Binding(info.BindingName)
         {
            Converter = new NavigatorToBrushConverter(info.Foreground, info.ForegroundWhenNavigator),
         };

         elementStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, bindingFG));
         //ctr.style => setter => (dependency property, real value or binding);

         //@ 目前是採用Wrap上
         if (info.IsWrap == true)
            elementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

         column.ElementStyle = elementStyle;
         column.Width = info.Width;
         return column;

      }//function





      /// <summary>公用的RowStyle，適用於所有的情況，繫結的物件需要實作"DataLayer"。
      /// 第一組DataGrid的RowStyle，需求：繫結屬性要有DataLayer，從0~3，包含了背景色，字體大小，字體重量。
      /// </summary>
      public static readonly Style DataGridRowStyle1;

      static DataGridHelper()
      {
         DataGridRowStyle1 = GetRowStyle1();          //建立一個通用型的RowStyle

      }

      /// <summary>取得一種泛用，預設的Rowstyle</summary>
      /// <returns></returns>
      private static Style GetRowStyle1()
      {
         //@ 建立RowStyle
         Style rowStyle = new Style();

         Binding bindingBK = new Binding();          //預設的繫結物件需要符合IRowDI
         bindingBK.Converter = new RowDIToBackgroundConverter();        //公用的背景色轉檔。
         Setter setter = new Setter(DataGridRow.BackgroundProperty, bindingBK);
         rowStyle.Setters.Add(setter);

         //@ 設定
         Binding bindingFont = new Binding();
         bindingFont.Converter = new RowDIToFontSizeConverter();        //字型大小
         Setter setterFont = new Setter(DataGridRow.FontSizeProperty, bindingFont);
         rowStyle.Setters.Add(setterFont);

         //@ 設定字體的重量
         Binding bindingFontWeight = new Binding();
         bindingFontWeight.Converter = new RowDIToFontWeightConverter();        //字體重量
         Setter setterFontWeight = new Setter(DataGridRow.FontWeightProperty, bindingFontWeight);
         rowStyle.Setters.Add(setterFontWeight);

         //@ 加入可視性。
         Binding bindingVisibility = new Binding("Visibility");
         Setter setterVisibility = new Setter(DataGridRow.VisibilityProperty, bindingVisibility);
         rowStyle.Setters.Add(setterVisibility);

         //@ 設定padding，試圖拉大間距，但沒有作用。
         //Setter setterPadding = new Setter(DataGridRow.MarginProperty, new Thickness(1,10,1,10));
         //rowStyle.Setters.Add(setterPadding);

         //@  設定rowHeight，有作用。但內容就沒有置中了。
         //Setter setterHeight = new Setter(DataGridRow.HeightProperty, 40.0);
         //rowStyle.Setters.Add(setterHeight);

         //@ 將內容置中，但沒有作用。
         Setter setterContentVertical = new Setter(DataGridRow.VerticalContentAlignmentProperty, VerticalAlignment.Center);
         rowStyle.Setters.Add(setterContentVertical);

         Setter setterVertical = new Setter(DataGridRow.VerticalContentAlignmentProperty, VerticalAlignment.Center);
         rowStyle.Setters.Add(setterVertical);

         return rowStyle;
      }

   }
}

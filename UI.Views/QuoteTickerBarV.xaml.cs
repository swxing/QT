using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QT.UI.ViewModels;
using vm=QT.UI.ViewModels;

namespace QT.UI.Views
{
   /// <summary>表示Dashboard上方的個股View</summary>
   public partial class QuoteTickerBarV : UserControl
   {
      //沒有使用傳統的Binding方式來更新UI，而是直接在ViewModel的PropertyChanged事件中更新UI.
      //這樣做的好處是可以更靈活地控制UI的更新邏輯，缺點是會增加View和ViewModel之間的耦合度.
      //傳統上：價格欄位好了。需要binding：價格。還有顏色。

      private readonly UI.Converters.PriceToColorConverter priceToColorConverter = new UI.Converters.PriceToColorConverter();
      
      public QuoteTickerBarV()
      {
         InitializeComponent();

         // MVVM模式，當DataContext改變時，設定PropertyChanged事件處理程序
         this.DataContextChanged += (s, e) =>
         {
            var vm = this.DataContext as vm.QuoteTickerVM 
               ?? throw new InvalidOperationException("DataContext必須是QuoteTickerVM");

            this.UpdateQuoteTextBlock();


            vm.PropertyChanged += (s, e) =>
            {
               //如果價格有變動，則更新整個跑馬燈的內容
               if (e.PropertyName == nameof(vm.Last))
                  this.UpdateQuoteTextBlock();
            };
         };



         #region 以下可以說是悲劇的Binding寫法範例，為了一行寫了無數的範例呀…

         //////////進行屬性的繫結, 用了nameof與字串混合的方式.可做參考
         ////////this.tebName.SetBinding(TextBlock.TextProperty, new Binding(nameof(ViewModels.QuoteTickerVM.Name)) );
         ////////this.tebSymbol.SetBinding(TextBlock.TextProperty, new Binding("Symbol") { StringFormat="({0})"});
         ////////this.tebLast.SetBinding(TextBlock.TextProperty, new Binding("Last") );
         ////////this.tebLast.SetBinding(TextBlock.ForegroundProperty, new Binding("Change") { Converter = priceToColorConverter } );
         ////////this.tebChange.SetBinding(TextBlock.TextProperty, new Binding("Change") );
         ////////this.tebChange.SetBinding(TextBlock.ForegroundProperty, new Binding("Change") { Converter = priceToColorConverter } );

         //////////ChangeRate採用格式化字串的方式來顯示 例如：(-5.2%)
         ////////this.tebChangeRate.SetBinding(TextBlock.TextProperty, new Binding("ChangeRate") { StringFormat="({0:+0.00%;-0.00%;0.00%})" } );
         ////////this.tebChangeRate.SetBinding(TextBlock.ForegroundProperty, new Binding("Change") { Converter = priceToColorConverter } );

         ////////this.tebVolume.SetBinding(TextBlock.TextProperty, new Binding(nameof(QuoteTickerVM.Volume)) );
         #endregion


      }





      private void UpdateQuoteTextBlock()
      {
         this.tebBar.Inlines.Clear();
         var inline = CreateQuoteTextBlock();
         this.tebBar.Inlines.Add(inline);
      }

      




      private Inline CreateQuoteTextBlock()
      {
         var vm = this.DataContext as vm.QuoteTickerVM;
         if(vm==null)
            throw new InvalidOperationException("DataContext必須是QuoteTickerVM");

         var span = new Span();        //Span是行內元素的容器，可以包含多個Run或其他行內元素。
         span.Background = Brushes.Black;          //指行內元素的背景色，但行內元素本身如果有背景色設定，會蓋過這個設定。除非設為null.

         span.Inlines.Add(Create(vm.Symbol + " ", 32, Brushes.White, Brushes.Transparent, FontWeights.Bold));
         span.Inlines.Add(Create(vm.Name+" ", 32, Brushes.White, Brushes.Transparent, FontWeights.Bold));

         //根據漲跌來決定顏色
         var fg = Res.UpBrush;
         
         if (vm.Change > 0)
            fg = Res.UpBrush;
         else if (vm.Change < 0)
            fg = Res.DownBrush;
         else
            fg = Res.FlatBrush;

         string msg=QT.Core.PriceFormatHelper.FormatByPrice(vm.Last);
         span.Inlines.Add(Create(msg+"  ", 22, fg, Brushes.Transparent, FontWeights.Bold));

         msg= QT.Core.PriceFormatHelper.FormatByPrice(vm.Change);
         span.Inlines.Add(Create(msg + "  ", 22, fg, Brushes.Transparent, FontWeights.Normal));

         msg = vm.ChangeRate.ToString("P2");
         span.Inlines.Add(Create( msg + "  ", 22, fg, Brushes.Transparent, FontWeights.Normal));
         //span.Inlines.Add(Create("12345678 ", 22, Brushes.White, Brushes.Transparent, FontWeights.Normal));
         
         return span;
      }





      /// <summary>建立單獨的一個Run，</summary>
      /// <param name="text"></param>
      /// <param name="fontSize"></param>
      /// <param name="foreground"></param>
      /// <param name="background"></param>
      /// <param name="weight"></param>
      /// <returns></returns>
      private static Run Create(    string text,    double fontSize,    Brush foreground,    Brush? background,    FontWeight weight)
      {
         var run= new Run(text)
         {
            FontSize = fontSize,
            FontWeight = weight,
            Foreground = foreground,
            Background = background,  
         };

         return run;
      }
   }
}

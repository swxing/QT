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

namespace QT.UI.Views
{
   /// <summary>
   /// QuoteTickerBarV.xaml 的互動邏輯
   /// </summary>
   public partial class QuoteTickerBarV : UserControl
   {
      public QuoteTickerBarV()
      {
         InitializeComponent();

         var inline = CreateQuoteTextBlock();
         this.tebBar.Inlines.Add(inline);
         this.Background = Brushes.Black;
         this.tebBar.VerticalAlignment = VerticalAlignment.Center;

      }

      private Inline CreateQuoteTextBlock()
      {
         var span = new Span();
         span.Background = Brushes.Black;          //指行內元素的背景色，但行內元素本身如果有背景色設定，會蓋過這個設定。除非設為null.
         span.Inlines.Add(Create("2330 ", 32, Brushes.White, Brushes.Transparent, FontWeights.Bold));
         span.Inlines.Add(Create("台積電 ", 32, Brushes.White, Brushes.Transparent, FontWeights.Bold));
         span.Inlines.Add(Create("550.0 ", 22, Brushes.YellowGreen, Brushes.Transparent, FontWeights.Bold));
         span.Inlines.Add(Create("+5.0 ", 22, Brushes.YellowGreen, Brushes.Transparent, FontWeights.Normal));
         span.Inlines.Add(Create("(+0.92%) ", 22, Brushes.YellowGreen, Brushes.Transparent, FontWeights.Normal));
         span.Inlines.Add(Create("12345678 ", 22, Brushes.White, Brushes.Transparent, FontWeights.Normal));
         
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
            Background = background
         };
         return run;
      }
   }
}

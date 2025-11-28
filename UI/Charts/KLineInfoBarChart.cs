using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using QT.Data;

namespace QT.UI.Charts
{
   public class KLineInfoBarChart : FrameworkElement
   {
      public Bar? CurrentBar { get; set; }

      DashboardState _state = null!;
      public required DashboardState State
      { 
         get=> _state;
         set { 
            _state = value;
            _state.SelectedDateChanged += (s, e) => 
            {
               //get bar by date
               var bar = QT.Data.Repository.DataService.GetBarSet(this.State.Symbol, this.State.Interval);
               var b = bar.FindBar(e.Date, FindDirection.Forward);
               this.Update(b);
            };


         }
      }


      public void Update(Bar? bar)
      {
         CurrentBar = bar;
         InvalidateVisual();    // 只有這個控制項會重畫，不會動到 K 線
      }

      protected override void OnRender(DrawingContext dc)
      {
         base.OnRender(dc);

         if (CurrentBar == null)
            return;

         var bar = CurrentBar;

         double change = bar.Close - bar.Open;
         double changeRate = bar.Open != 0 ? change / bar.Open : 0;

         var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
         var typeface = new Typeface("微軟正黑體");
         double fontSz = 16.0;

         //-----------------------------------------
         // 左邊的白色文字 (日期 / OHLC / 量)
         //-----------------------------------------

         string left =
             $"{bar.TimeStamp:yyyy年M月d日} " +
             $"開 {bar.Open:F2} 高 {bar.High:F2} 低 {bar.Low:F2} 收 {bar.Close:F2} " +
             $"量 {bar.Volume:N0} ";

         var leftText = new FormattedText(
             left,
             CultureInfo.CurrentUICulture,
             FlowDirection.LeftToRight,
             typeface,
             fontSz,
             Brushes.White,
             dpi);

         dc.DrawText(leftText, new Point(0, 0));

         //-----------------------------------------
         // 右邊：漲時紅、跌時綠
         //-----------------------------------------

         string sign = change >= 0 ? "+" : "";
         string right =
             $"漲幅 {sign}{change:F2} ({sign}{changeRate * 100:F2}%)";

         Brush changeBrush = change >= 0 ? Brushes.Red : Brushes.LimeGreen;

         var rightText = new FormattedText(
             right,
             CultureInfo.CurrentUICulture,
             FlowDirection.LeftToRight,
             typeface,
             fontSz,
             changeBrush,
             dpi);

         // 接在左邊後面
         double x = leftText.WidthIncludingTrailingWhitespace;
         dc.DrawText(rightText, new Point(x, 0));
      }
   }


}

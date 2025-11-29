using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QT.UI.Charts
{
   public class DashboardLid: System.Windows.FrameworkElement
   {
      //null!是null抑制運算子，表示我保證這個值不會是null。
      ChartViewState _state = null!;     //我保證這個值會在後續被設定好。這樣才不會有8618的警告。


      public DashboardLid()
      {
         IsHitTestVisible = false;      //這個控制項不會攔截滑鼠事件。
                                        //WPF的路由事件系統會先從最上層的元素開始往下找(到isHitTestVisible=true)，找到第一個能處理事件的元素來處理事件。
                                        //但在 Grid 中，事件不是「在所有子控件內一個一個輪流傳」，
                                        //而是「滑鼠底下那個最上層控件先被 hit，事件只在它和它的父層之間路由」。


      }


      /// <summary></summary>
      public required ChartViewState State
      {     //required表示這個屬性在物件初始化時一定要被設定好。。
         //required,可以用init與set來設定值, 但都要在建構器中使用。但set是可再次設定的。
         get { return this._state; }
         set
         {
            if (this._state == value)
               return;
            this._state = value;
            this._state.SelectedDateChanged += State_SelectionDateChanged;
            this._state.DragStarted += State_DragStarted;
         }
      }

      private void State_DragStarted()
      {
         //最主要是要能知道拖曳開始了。我們要重新繪制。(不要讓選取框出現)
         this.InvalidateVisual();
      }

      private void State_SelectionDateChanged(object? sender, DateTime e)
      {
         this.InvalidateVisual();

      }



      protected override void OnRender(DrawingContext dc)
      {

         //System.Diagnostics.Debug.WriteLine("進入");

         if (this._state == ChartViewState.Empty)
            return;

         base.OnRender(dc);
         if (this._state == null || string.IsNullOrEmpty(this._state.Symbol) )
            return;



         if (this._state.IsDrag)
            return;                  //拖曳中，不繪制十字線


         var mouseP = Mouse.GetPosition(this);

         //@ 水平線
         Point hP0 = new(0, mouseP.Y);
         Point hP1 = new(this.ActualWidth, mouseP.Y);
         dc.DrawLine(Res.CrossLinePen, hP0, hP1);


         #region @ 垂直線，需算出X
         //(只能繪制日期的正中央，所以一定是一格一格的))
         //////var his = TradingSet.GetTradingSet(this._state.Stock.Token);
         //////var index = his.IndexOfForward(this._state.StartDate);                     //圖上的第一個日期
         //////var index2 = his.IndexOfForward(this._state.SelectedDate);           //滑鼠選取的日期
         //////double adjustX = this._state.OffsetX + (index2 - index) * this._state.BarWidth + this._state.BarWidth * 1 / 2;
         //////Point vP0 = new Point(adjustX, 0);
         //////Point vP1 = new Point(adjustX, this.ActualHeight);
         //////drawingContext.DrawLine(Res.CrossLinePen, vP0, vP1);

         #endregion


         #region @ 繪制Rectangle(選取的日期)

         if (this._state.SelectedDate != null)
         {
            //var his = TradingSet.GetTradingSet(this._state.Stock.Token);
            var his = QT.Data.Repos.DataService.GetBarSet(this._state.Symbol, this._state.Interval);
            var index = his.IndexOfByDate(this._state.VisibleStart, Data.FindDirection.Forward);                     //圖上的第一個日期
            var index2 = his.IndexOfByDate(this._state.SelectedDate.Value, Data.FindDirection.Forward);           //滑鼠選取的日期
            double x0 = this._state.OffsetX + this._state.BarWidth * (index2 - index);
            double x1 = x0 + this._state.BarWidth;
            Point vP0 = new(x0, 0);
            Point vP1 = new(x1, this.ActualHeight);
            SolidColorBrush b = new(Color.FromArgb(30, 192, 192, 192));
            dc.DrawRectangle(b, null, new Rect(vP0, vP1));


            
            //繪制日期。
            var typeface = new Typeface("微軟正黑體");
            double fontSz = 16.0;
            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            var leftText = new FormattedText(this._state.SelectedDate.Value.ToString("yyyy/MM/dd"),

       CultureInfo.CurrentUICulture,
       FlowDirection.LeftToRight,
       typeface,
       fontSz,
       Brushes.White,
       dpi);

            double  x =x0 -(leftText.Width / 2) + (x1-x0)/2;
            var p = new Point(x, this.ActualHeight);

            Rect rt=new Rect(p, new Size(leftText.Width, leftText.Height)); ;
            dc.DrawRectangle(Brushes.DarkKhaki, null, rt);

            dc.DrawText(leftText, p);


            
         }
         #endregion


         #region 加入遮罩，遮罩的部份是左邊到選取的日期，右邊到最右邊。
         ////左邊遮罩
         //if (x0 > 0)
         //{
         //   Point mP0 = new Point(x1, 0);
         //   Point mP1 = new Point(this.ActualWidth, this.ActualHeight);

         //   drawingContext.DrawRectangle(Brushes.Black, null, new Rect(mP0, mP1));
         //}



         #endregion



         ////@=== 在正中央繪制出固定線。
         ////邏輯：偏移X，Bar的寬度。第幾個Bar

         ////找出中心點。再根據中心點找出應該是第幾個Bar
         //double x = rect.Width / 2;                //中心點
         //var centerIndex = (int)((x - this._state.OffsetX) / this._state.BarWidth);
         //x = centerIndex * this._state.BarWidth + this._state.BarWidth / 2;            //最後的X座標

         //Point cP0 = new Point(x, 0);
         //Point cP1 = new Point(x, rect.Height);
         //drawingContext.DrawLine(Res.GrayPen, cP0, cP1);



      }



   }
}

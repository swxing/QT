using QT.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QT.UI.Charts
{


   /// <summary>一個位於最頂端的透明層，用來處理滑鼠的訊息，含move, drag等，並再將結果設定到State屬性中。</summary>
    public class ChartInteractionLayer:System.Windows.Controls.Control
    {

      public required ChartViewState State { get; set; } = ChartViewState.Empty;
      Point _mouseDown_Point;
      DateTime _mouseDown_StartDate;
      double _mouseDown_OffsetX;


      public ChartInteractionLayer()
      {
         this.MouseWheel += Dashboard_MouseWheel;
         this.MouseDown += Dashboard_MouseDown;
         this.MouseUp += Dashboard_MouseUp;
         this.MouseMove += Dashboard_MouseMove;
      }


      /// <summary>當bar拉到最後時，呈現的Bar至少要劃面一半時的第一根。</summary>
      /// <returns></returns>
      public Bar GetStartBarWhenScrollEnd()
      {
         
         var bars = BarSet.GetBarSet(this.State.Symbol, this.State.Interval);
         double realBarWidth = this.barWidth * this.ScaleRate;
         int count = (int)(this.ActualWidth / realBarWidth / 2);          //可容納的bar數量
         var startBar = bars.Bars[bars.Bars.Count - count - 1];            //取得畫面為一半時的起始Bar
         return startBar;
      }




      public void GoTo(DateTime date, FlowDirection direction)
      {
         var barFirst = GetStartBarWhenScrollEnd();          //此為最大的StartDate。(因為bar的最後一根不可少於畫面的一半)
         
         if (date > barFirst.TimeStamp)
            date = barFirst.TimeStamp;
         var bars = BarSet.GetBarSet(this.State.Symbol, this.State.Interval);
         if (bars.Bars.Count == 0)          //因為沒有資料
            return;

         //@日期再修正。
         if (date < bars.Bars[0].TimeStamp)        //本來是用first，但出現了CA1826的警告。改用索引方式。[0]
            date = bars.Bars[0].TimeStamp;
         if (date > bars.Bars[bars.Bars.Count - 1].TimeStamp)
            date = bars.Bars[bars.Bars.Count - 1].TimeStamp;

         Bar? bar;

         if (direction == FlowDirection.LeftToRight)        //向後
            bar = bars.FindBar(date, FindDirection.Backward);
         else
            bar = bars.FindBar(date, FindDirection.Forward);

         if (bar == null)
            return;

         //修正bar
         this.State.VisibleStart = bar.TimeStamp;
         this.State.SelectedDateTime = this.GetSelectedDate();

         //@ 針算出新的StartDate
         if (this.State.IsFixedDateLine == true)
         {
            //求出Dashboard上，StartDate最大的日期
            double percentWidth = this.ActualWidth * this.State.FixedDateLinePosition;
            percentWidth -= this.State.OffsetX;                     //扣掉偏移量
            var realBarWidth = this.barWidth * this.ScaleRate;
            int count = (int)(percentWidth / realBarWidth);          //可容納的bar數量

            bar = bars.Bars[bars.Bars.Count - count - 1];
            if (bar == null)
               return;

            if (this.State.VisibleStart > bar.TimeStamp)             //當起始日大於限制時
               this.State.VisibleStart = bar.TimeStamp;          //設定新的StartDate
         }

      }


      /// <summary>更新所選的日期。會觸發state的SelectedDate事件。</summary>
      private DateTime? GetSelectedDate()
      {
         double x = 0.0;
         if (this.State.IsFixedDateLine == false)
         {
            var p = Mouse.GetPosition(this);
            x = p.X;
         }
         else
         {
            x = this.ActualWidth * this.State.FixedDateLinePosition;
         }

         var item = this.GetBarFromX(x);        //get item from mouse position.
         if (item == null)
            return null;

         return item.TimeStamp;
      }



      private void Dashboard_MouseWheel(object sender, MouseWheelEventArgs e)
      {


         //@ there are two status in mouseWheel, one is drag on dashboard, the other is scale dateTime        
         if (Keyboard.IsKeyDown(Key.LeftCtrl))
         {
            if (e.Delta > 0)
               this.ScaleRate += 0.05d;
            else
               this.ScaleRate -= 0.05d;
         }
         else
         {
            var date = this.State.VisibleStart;

            if (e.Delta > 0)
            {
               date = date.AddMonths(-1);
               this.GoTo(date, FlowDirection.RightToLeft);
            }
            else
            {
               date = date.AddMonths(1);
               this.GoTo(date, FlowDirection.LeftToRight);
            }

         }

         this.State.RequestRefreshChartsUI();
      }


      private void Dashboard_MouseDown(object sender, MouseButtonEventArgs e)
      {

         this._mouseDown_Point = e.GetPosition(this);
         this._mouseDown_StartDate = this.State.VisibleStart;
         this._mouseDown_OffsetX = this.State.OffsetX;
         this.State.IsDrag = true;                                                                //indicate the action of dashboard is in draging~


      }

      /// <summary>處理Dashboard位移與縮放</summary>
      private void Dashboard_MouseUp(object sender, MouseButtonEventArgs e)
      {
         this._mouseDown_Point = e.GetPosition(this);
         this._mouseDown_StartDate = this.State.VisibleStart;
         this._mouseDown_OffsetX = this.State.OffsetX;
         this.State.IsDrag = false;
         this.State.SelectedDateTime = this.GetSelectedDate();
      }


      /// <summary>處理Dashboard位移與縮放</summary>
      private void Dashboard_MouseMove(object sender, MouseEventArgs e)
      {

       

         if (this.State == ChartViewState.Empty)
            return;

         //如果不是拖曳，則只更新選取的日期
         if (this.State.IsDrag == false)
         {
            this.State.SelectedDateTime = this.GetSelectedDate();
            return;
         }



         System.Diagnostics.Debug.WriteLine($"Mouse Move - IsDrag: {this.State.IsDrag}");

         #region 如果正在拖曳，則進行拖曳的動作
         if (this.State.IsDrag == true)
         {
            //非左鍵按下就不處理
            if (Mouse.LeftButton != MouseButtonState.Pressed)
               return;

            //@ 表示日期的決定是由滑鼠移動的點決定
            if (this.State.IsFixedDateLine == false)       //表示是滑鼠的點決定日期。
            {
               var point = Mouse.GetPosition(this);            //滑鼠的點
               double x移動距離 = point.X - this._mouseDown_Point.X;        //跟第一次點下去相比，位移了多少。
               x移動距離 += this._mouseDown_OffsetX;                                    //再加回原來的偏移量。
               int 移動數量 = (int)(x移動距離 / this.State.BarWidth);                 //跟第一次點下去相比，移動了多少bar數量



               //移動的距離沒問題。

               var bars =  BarSet.GetBarSet(this.State.Symbol, this.State.Interval);
               int startBarIndex = bars.IndexOfByDate(this._mouseDown_StartDate, FindDirection.Forward);

               startBarIndex -= 移動數量;           //最新的startBarIndex

               if (startBarIndex >= bars.Bars.Count)                                               //index can not more than his count....
                  startBarIndex = bars.Bars.Count - 1;
               if (startBarIndex < 0)
                  startBarIndex = 0;

               //System.Diagnostics.Debug.WriteLine($"位移: {x移動距離}; 寬度: {this.State.BarWidth}, 移動數量: {移動數量}, 索引: {startBarIndex}");

               //問題：我們不希望最後一根出現在最左側，而是出現中間。
               //需要知道若是最後一根出現在中間，起始日要怎麼算？
               //int middleCount = (int)(this.ActualWidth / (this.State.BarWidth * 2));   //中間位置可容納的bar數量
               //int middleIndex = bars.Bars.Count - middleCount - 1;         //最後一根在中間時，起始日的index
               //if (startBarIndex > middleIndex)                                               //當起始日大於限制時
               //   startBarIndex = middleIndex;                                           //設定新的startBarIndex


               //var startDate = bars.Bars[startBarIndex].TimeStamp;
               //var maxStartDate= this.GetStartBarWhenScrollEnd();
               //if(startDate > maxStartDate)          //當起始日大於限制時
               //   startDate = maxStartDate;
               //this.State.VisibleStart = startDate;


               this.State.OffsetX = x移動距離 % this.State.BarWidth;                               //餘數為偏移量
               //this.State.OffsetX = 0;
               this.State.VisibleStart = bars.Bars[startBarIndex].TimeStamp;
               this.State.SelectedDateTime = this.GetSelectedDate();
               this.State.RequestRefreshChartsUI();

            }
            else   //選取日期的決定是由Dashboard的固定線…
            {

               //route: 拖曳=>  決定日期(就中心點)=>設定StartDate=>diagram重新繪制
               var point = Mouse.GetPosition(this);                           //現在滑鼠的位置
               var offsetX = point.X - this._mouseDown_Point.X;         //滑鼠移動的距離
               var realBarWidth = this.barWidth * this.ScaleRate;
               int offsetCount = (int)(offsetX / realBarWidth);               //移動的bar數量
               offsetCount = -offsetCount;                                                   //因為是反向的
               var bars = BarSet.GetBarSet(this.State.Symbol, this.State.Interval);
               var bar = bars.FindBar(this._mouseDown_StartDate, FindDirection.Forward, offsetCount);     //找出實際的日期了。
               if (bar == null)
                  return;
               this.State.VisibleStart = bar.TimeStamp;


               //求出Dashboard上，StartDate最大的日期
               double percentWidth = this.ActualWidth * this.State.FixedDateLinePosition;
               percentWidth -= this.State.OffsetX;                     //扣掉偏移量
               int count = (int)(percentWidth / realBarWidth);          //可容納的bar數量
               var trading = bars.FindBar(bars.Bars.Last().TimeStamp, FindDirection.Forward, -(count));  //從最後一筆交易往前找偏移後的交易，就是第一天的交易了。
               if (trading == null)
                  return;

               if (this.State.VisibleStart > trading.TimeStamp)             //當起始日大於限制時
                  this.State.VisibleStart = trading.TimeStamp;          //設定新的StartDate
               this.State.SelectedDateTime = this.GetSelectedDate();

               this.State.RequestRefreshChartsUI();
            }
         }
         #endregion 

      }



      double _scaleRate = 1.0;

      /// <summary>個bar的寬度，預設為20，設成10時縮放到最小會看不見。      </summary>
      readonly double barWidth = 20;


      /// <summary>根據X座標來判斷那天的交易被選取。</summary>
      public Bar? GetBarFromX(double x)
      {
         if (this.State.Symbol == null)
            return null;

         var bars = BarSet.GetBarSet(this.State.Symbol, this.State.Interval);

         x = x - this.State.OffsetX;                                            //把偏移去除掉。
         double indexD = x / (barWidth * this.ScaleRate);          //縮放後，bar的寬度
         var index = (int)Math.Floor(indexD);                     //無條件捨去去。Celling是無條件進入法。
         index = bars.IndexOfByDate(this.State.VisibleStart, FindDirection.Forward) + index;

         if (index < 0)
            return bars.Bars.First();
         else if (index >= bars.Bars.Count)
            return bars.Bars.Last();
         else
            return bars.Bars[index];

      }

      /// <summary>Kbar的縮放的倍率。最大為2，最小為0.1。步進建議為0.1。
      /// 改變時同時也會改變第一筆資料的索引值。</summary>
      public double ScaleRate
      {
         get { return this._scaleRate; }
         set
         {

            if (value > 2 || value < 0.1)          // 2跟0.1是測試過，較佳的視覺值
               return;
            if (value == this._scaleRate)
               return;

            var point = Mouse.GetPosition(this);
            var currentItem = this.GetBarFromX(point.X);           //get selection item

            var barWidth = this.barWidth * value;
            int count = (int)(point.X / barWidth);                         //current width can hold how many bar

            var bars = BarSet.GetBarSet(this.State.Symbol, this.State.Interval);
            var index = bars.Bars.ToList().IndexOf(currentItem!);               //!表示不可能為null，就開發人員自己負責。

            index = index - count + 1;
            if (index < 0)
               index = 0;
            double offsetX = (point.X % barWidth) + barWidth / 2;
            this._scaleRate = value;
            this.State.BarWidth = barWidth;             //state不知道縮放，只知道bar的寬度。
            this.State.VisibleStart = bars.Bars[index].TimeStamp;              //起始日。
            this.State.OffsetX = offsetX;
         }//set
      }

      
      protected override void OnRender(DrawingContext dc)
      {

         base.OnRender(dc);

         // 畫滿整個控制項為黑色，否則會無法接收滑鼠事件。
         dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));
      }


   }//cls
}//ns

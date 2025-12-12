using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace QT.UI.Charts
{
   public class DateTimeChart:FrameworkElement
   {

      TextBlock _tbDate = null;


      public DateTimeChart()
      {

      }

      ChartViewState _state = null!;

      public  ChartViewState State
      {
         get => this._state;
         set
         {
            this._state = value; 
            this._state.RefreshChartsUiRequested += () => this.InvalidateVisual();

         }


      }




      protected override void OnRender(DrawingContext drawingContext)
      {
         base.OnRender(drawingContext);
         Rect rect = new Rect(0, 0, ActualWidth, ActualHeight);
         drawingContext.DrawRectangle(Brushes.DarkBlue, null, rect);

         //State，有StartDate。跟selectDate

         //@ 繪制日期
         if(this._state==ChartViewState.Empty)
            return;

         

         var his = QT.Data.DataService.GetBarSet(this.State.Symbol, this.State.Interval);
         int count = (int)(this.ActualWidth / this.State.BarWidth);       //how many bar can inside of the control
         var index = his.IndexOfByDate(this.State.VisibleStart, Data.FindDirection.Forward);

         if (index == -1)
            index = 0;
         if (index + count >= his.Bars.Count)
            count = his.Bars.Count - index;

         var items = his.GetRange(index, count);
         int month = -1;


         double _lastX = -1;

         foreach (var item in items)
         {
            //@ 寫入各月份
            if (item.TimeStamp.Month != month)
            {
               index = items.IndexOf(item);
               month = item.TimeStamp.Month;
               string msg = "";

               if (index == 0)
                  msg = item.TimeStamp.ToString("D");
               else if (month == 1 || month == 6)
                  msg = item.TimeStamp.ToString("Y");         //2121年6月
               else
                  msg = item.TimeStamp.ToString("MMMM");      //7月
               var ft = Tools.GetFormattedText(msg, Brushes.White);

               double x = this.State.OffsetX + State.BarWidth * index;            //get x position
               x = x - (ft.Width) / 2;
               if (x < 0)
                  x = 0;
               if (x < _lastX)
                  continue;
               Point point = new Point(x, 2);
               _lastX = x + ft.Width;
               drawingContext.DrawText(ft, point);
            }
         }




      }//fun
   }
}

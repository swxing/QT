using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
namespace QT.UI.Charts
{
   public class VolumnChart:FrameworkElement
   {

      public VolumnChart()
      { 
      
      }

      ChartViewState _state = null!;
      public required ChartViewState State
      {
         get => _state;
         set { 
         
            this._state = value;
           this._state.RefreshChartsUiRequested += () => this.InvalidateVisual();   
         }
      }

      protected override void OnRender(DrawingContext drawingContext)
      {

         base.OnRender(drawingContext);
         Rect rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
         drawingContext.DrawRectangle(Brushes.Transparent, Res.DiaBorPen, rect);          //draw border

         rect.Inflate(0, -4);          //reserve 上下4pixel

         if(this._state==ChartViewState.Empty)
            return;

         var bars = QT.Data.DataService.GetBarSet(this._state.Symbol, this._state.Interval);
         
         if (bars.Bars.Count == 0)
            return;

         var index = bars.IndexOfByDate(this.State.VisibleStart, Data.FindDirection.Forward);
         if (index == -1)
            index = 0;
         int count = (int)(rect.Width / this.State.BarWidth);
         if (index + count >= bars.Bars.Count)           //index cannot >= count, 
            count =bars.Bars.Count - index;

         var items = bars.GetRange(index, count);
         long volumnMax = items.Max(item => item.Volume);            //選出最大的值。


         var range = bars.Bars.Where(temp => temp.TimeStamp > items[0].TimeStamp.AddYears(-1)
         && temp.TimeStamp < items[items.Count-1].TimeStamp);
         volumnMax = range.Max(item => item.Volume);


         //@ Draw everyDay Volumn~
         foreach (var item in items)
         {
            //@ 算出今天成交量的Rect的兩個點。
            index = items.IndexOf(item);                      //WPF中，左上角為原點。
            //drawingContext.PushOpacity(0.5);                       //成交量為半透明。      //哇，這個非常的耗效能呀。
            Point leftBottomPoint = new Point(index * this.State.BarWidth, rect.Bottom);               //左下角設為p1
            double height = rect.Height * ((double)item.Volume / volumnMax);
            //height = height * 0.96;                                              //避免高度觸頂，所以只給0.96倍。
            double x = leftBottomPoint.X + this.State.BarWidth;
            double y = leftBottomPoint.Y - height;
            Point volumnRightTopPoint = new Point(x, y);       //右上角

            //@ 位移時間軸的差
            leftBottomPoint.Offset(this.State.OffsetX, 0);              //左上角。
            volumnRightTopPoint.Offset(this.State.OffsetX, 0);

            //@ 
            Rect barRect = new Rect(leftBottomPoint, volumnRightTopPoint);
            Pen pen = new Pen(Brushes.WhiteSmoke, 1);
            barRect.Inflate(-1, 0);

            if (item.Close > item.Open)
               drawingContext.DrawRectangle(Res.UpBrush, null, barRect);
            else
               drawingContext.DrawRectangle(Res.DownBrush, null, barRect);

         }//每天的成交量。

         //////////foreach (var indicator in this.Indicators)
         //////////{
         //////////   if (indicator.IsVisible == false)
         //////////      continue;
         //////////   List<Point> ma1Points = new List<Point>();
         //////////   foreach (var dataProvider in indicator)
         //////////   {
         //////////      int dataProviderIndex = indicator.IndexOf(dataProvider);
         //////////      var pen = new Pen(dataProvider.Foreground, dataProvider.Width);

         //////////      foreach (var item in items)
         //////////      {
         //////////         int Index = items.IndexOf(item);
         //////////         var value = dataProvider.GetValue(item.Date);
         //////////         double x = State.BarWidth * Index + this.State.BarWidth * 1 / 2;               //將線置中
         //////////         x += State.OffsetX;
         //////////         double height = rect.Height * ((double)value / volumnMax);
         //////////         //height = height * 0.96;                                              //避免高度觸頂，所以只給0.96倍。
         //////////         Point maPoint = new Point(x, rect.Bottom - height);
         //////////         ma1Points.Add(maPoint);
         //////////      }

         //////////      if (ma1Points.Count <= 1)
         //////////         return;

         //////////      var curvePoints = Tools.MakeCurvePoints(ma1Points.ToArray(), 0.1);
         //////////      PathGeometry geo = Tools.MakeBezierPath(curvePoints);
         //////////      drawingContext.DrawGeometry(null, pen, geo);
         //////////   }
         //////////}

         #region @ 繪制右側成交量數值。

         Rect priceRet = new Rect(this.ActualWidth - 60, 0, 60, this.ActualHeight);
         Brush lineBrush = new SolidColorBrush(Color.FromArgb(150, 100, 100, 100));
         Pen pricePen = new Pen(lineBrush, 1);

         drawingContext.DrawRectangle(Brushes.Black, null, priceRet);
         drawingContext.DrawLine(pricePen, priceRet.TopLeft, priceRet.BottomLeft);

         #endregion

      }

   }
}

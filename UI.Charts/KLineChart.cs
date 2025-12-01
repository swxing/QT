using QT.Data;
using QT.Data.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using d = System.Diagnostics.Debug;

namespace QT.UI.Charts
{
   public class KLineChart:FrameworkElement
   {

      //private KLineInfoBarChart _info;

      public KLineChart()
      {
         



      }




      List<Data.Indicators.Indicator> _indicators = new List<Data.Indicators.Indicator>();




      public void AddIndicator(Data.Indicators.Indicator indicator)
      {
         _indicators.Add(indicator);
      }




      ChartViewState _state = null!;

      public ChartViewState State
      {
         get => _state;

         set
         {
            //進行註冊
            _state = value;
            _state.RefreshChartsUiRequested += () => this.InvalidateVisual();

         }//set

      }//state

      float _minPrice = float.MaxValue;
      float _maxPrice = float.MinValue;

      readonly double _infoBarHeight = 36;

      int _count = 0;

      protected override void OnRender(DrawingContext dc)
      {

         d.WriteLine($"KLineChart OnRender {_count++} times.");

         if (this.State == null || string.IsNullOrEmpty(State.Symbol))
            return;

         base.OnRender(dc);


         //@ 背景
         var rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
         dc.DrawRectangle(Brushes.Transparent, null, rect);

         //找出要呈現的bars
         int count = (int)(this.ActualWidth / State.BarWidth);
         var barSet = DataService.GetBarSet(State.Symbol, State.Interval);
         int index = barSet.IndexOfByDate(State.VisibleStart, FindDirection.Forward);
         //確保index不會超過範圍
         if (index < 0)
            index = 0;
         if (index + count > barSet.Bars.Count)
            count = barSet.Bars.Count - index;

         var bars = barSet.GetRange(index, count);


         System.Diagnostics.Debug.WriteLine($"bar: {bars[0].TimeStamp.Date}");

         //計算出價格區間
         //@ decide min and max value of the itmes
         var minPrice = bars.Min(temp => temp.Low);                                //minmum price~
         var maxPrice = bars.Max(temp => temp.High);                             //maximun price
         //@ for avoid shake layout, now usually change min and max price...
         if (minPrice < this._minPrice || maxPrice > this._maxPrice || minPrice * 1.2 > this._minPrice || maxPrice * 0.8 < this._maxPrice)
         {
            //儘量保持不晃版，但也要做最有效的利用：最高/低價不能起過界限，或低於界限的2成。
            //如果用呈現區間的最高/最低價來設定範圍，則在拖拉時會可能因為最高/最低價的變動而導致整個圖表範圍跳動，這樣使用者體驗不好。
            this._minPrice = minPrice * 0.95f;                                                                                        //extend the range
            this._maxPrice = maxPrice * 1.05f;                                                                                          //extend the range
         }

         index = 0;
         //繪製bars
         foreach (var bar in bars)
         {
            #region K線中的影線。
            double KShadowLineX = State.BarWidth * index + this.State.BarWidth * 1 / 2;         //將線置中
            double KShadowLineY0 = this.GetY(bar.High);
            double KShadowLineY1 = this.GetY(bar.Low);
            Point KShadowLineP1 = new(KShadowLineX, KShadowLineY0);
            Point KShadowLineP2 = new(KShadowLineX, KShadowLineY1);
            KShadowLineP1.Offset(State.OffsetX, 0);
            KShadowLineP2.Offset(State.OffsetX, 0);

            if (bar.Close >= bar.Open)
               dc.DrawLine(Res.RisePen, KShadowLineP1, KShadowLineP2);
            else
               dc.DrawLine(Res.DownPen, KShadowLineP1, KShadowLineP2);


            #endregion


            #region k線的實體
            //@ K線的實體框。實體框要比該Bar瘦小一點，左右各小0.1

            double x0 = this.State.BarWidth * index + this.State.BarWidth * 0.2;
            double x1 = this.State.BarWidth * index + this.State.BarWidth * 0.8;
            KShadowLineY0 = this.GetY(bar.Open);
            KShadowLineY1 = this.GetY(bar.Close);

            Point KBodyP1 = new(x0, KShadowLineY0);
            Point KBodyP2 = new(x1, KShadowLineY1);

            //@ 進行位移
            KBodyP1.Offset(State.OffsetX, 0);
            KBodyP2.Offset(State.OffsetX, 0);

            Rect kRect = new(KBodyP1, KBodyP2);
            if (kRect.Height == 0)           //如果最終為平盤，則此rect的height為零，在UI上沒有呈現，所以加1。
               kRect.Height = 1;

            if (bar.Close >= bar.Open)
               dc.DrawRectangle(Res.RiseBrush, null, kRect);
            else
               dc.DrawRectangle(Res.DownBrush, null, kRect);

            #endregion



            index++;
         }



         foreach (var indicator in this._indicators)
         {
            if (indicator.IsVisible == false)
               continue;
            foreach (var indicatorVisual in indicator)
            {
               var pen = new Pen(indicatorVisual.Foreground, indicatorVisual.Width);
               // var providerIndex = indicator.IndexOf(indicatorItem);
               int maIndex = -1;
               List<Point> maPoints = new List<Point>();
               foreach (var trading in bars)
               {
                  maIndex++;
                  var price = indicatorVisual.GetValue(trading.TimeStamp);
                  if (float.IsNaN(price))           //NaN不能用price==float.NaN來判斷。
                     continue;

                  double x = State.BarWidth * maIndex + this.State.BarWidth * 1 / 2;               //逐一取得X 的位置。
                  x += State.OffsetX;                                               //加上偏移量。
                  double y = this.GetY(price);                                       //取得Y的正確位置。


                  Point maPoint = new Point(x, y);
                  maPoints.Add(maPoint);
               }

               var curvePoints = Tools.MakeCurvePoints(maPoints.ToArray(), 0.1);          //將點轉換成曲線。
               if (curvePoints == null)
                  continue;

               PathGeometry geo = Tools.MakeBezierPath(curvePoints);
               dc.DrawGeometry(null, pen, geo);                   //繪制。

            }
         }





         #region @ 繪制價格線
         //Rect priceRet = new Rect(this.ActualWidth - 50, _infoBarHeight, 50, this.ActualHeight-this._infoBarHeight);
         Rect priceRet = new(this.ActualWidth - 60, 0, 60, this.ActualHeight);

         Pen pricePen = new(Res.DownBrush, 1);

         dc.DrawRectangle(Brushes.Black, null, priceRet);
         dc.DrawLine(pricePen, priceRet.TopLeft, priceRet.BottomLeft);

         float priceLine = (int)this._minPrice;
         //drawingContext.PushOpacity(0.2 );                //花效能…

         double lastY = this.ActualHeight;
         while (true)
         {
            //應該找出最大的級距來繪制。
            //台股的股價升降的單位則依股價範圍不同而異，採6個級距方式，
            //股價未滿10元者,升降單位為0.01元，   //股價10元至未滿50元者為0.05元，
            //50元至未滿100元者為0.1元，   //100元至未滿500元者為0.5元，
            //500元至未滿1000元者為1元，   //1000元以上者為5

            //@ 以5角為一個單位往上畫且兩條線的間隔最少要30pixel.
            priceLine += 0.5f;
            //priceLine = price_Line * 1.05f;            //此方法不可行, 5%是基於誰的5%？
            var y = this.GetY(priceLine);              //get y position
            if (y < _infoBarHeight)                         //上方是控制項了。
               break;
            if (lastY - y < 30)        //the gap of two lines need to more than 30 pixels.
               continue;

            //@ 價格線…
            lastY = y;
            Point p0 = new(0, y);
            Point p1 = new(priceRet.Left, y);
            dc.DrawLine(pricePen, p0, p1);            //K線區的價格線

            //@ Price Text
            var ft = Tools.GetFormattedText(priceLine.ToString("N1"), Brushes.White, 12);
            Point textPoint = new(p1.X + 5, p1.Y - (ft.Height / 2));
            dc.DrawText(ft, textPoint);

         }
         #endregion







      }

      /// <summary>根據一個金額，取得在Graph中Y的位置。</summary>
      /// <param name="money"></param>
      /// <returns></returns>
      private double GetY(float money)
      {
         double pointY = (double)((money - this._minPrice) / (this._maxPrice - this._minPrice)) * this.ActualHeight;       //影線的起始點。
         return this.ActualHeight - pointY;
      }
   }
}

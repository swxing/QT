using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
namespace QT.UI.Charts
{
   public class SelectionPlot
   {
      public SelectionPlot(DateTime date, Brush brush)
      {
         Date = date;
         Brush = brush;
      }
      public DateTime Date { get; set; }
      public Brush Brush { get; set; } = Brushes.Transparent;
   }

   public  class BackgroundChart:FrameworkElement
   {


      /// <summary>for selection date and plot... </summary>
      List<SelectionPlot> _selectionPlots = new List<SelectionPlot>();
      ChartViewState _state;

      public ChartViewState State
      {
         get=> this._state;
         set
         { 
            _state= value;
            _state.RefreshChartsUiRequested += () => this.InvalidateVisual();
            
         }
      }

      /// <summary>在加入 plot之後，並不會進行繪制。而是需要呼叫InvalidVisual才行。
      /// 如果要清除之前某日的Plot，請呼叫RemoveSelectPolt。若要全部清除Plots，則呼叫CleanSelectPlots
      /// </summary>
      /// <param name="plot"></param>
      public void AddSelectionPlots(List<SelectionPlot> plots)
      {
         this._selectionPlots.AddRange(plots);
      }

      public void RemoveSelectionPlot(DateTime date)
      {

         var item = this._selectionPlots.Find(temp => temp.Date == date);
         if (item != null)
            this._selectionPlots.Remove(item);
      }

      /// <summary>The visibility of Plot。Event Route are =>  MPService, RequestPlot, event fire => MainWindow => BaseDiagram</summary>
      public bool IsPlotVisible
      {
         get; set;
      } = true;


      public void ClearSelectionPlots()
      {
         this._selectionPlots.Clear();
      }


      protected override void OnRender(DrawingContext dc)
      {
         base.OnRender(dc);

         //@ draw control background
         Rect rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
         dc.DrawRectangle(Brushes.Black, null, rect);
         if (this._state == ChartViewState.Empty)
            return;

         
         var ts = QT.Data.Repos.DataService.GetBarSet(this._state.Symbol, this._state.Interval);
         var index = ts.IndexOfByDate(this._state.VisibleStart, Data.FindDirection.Forward);
         if (index == -1)
            return;

         int count = (int)(rect.Width / this._state.BarWidth);
         if (index + count > ts.Bars.Count)
            count = ts.Bars.Count - index;
         var items = ts.GetRange(index, count);
         int month = items[0].TimeStamp.Month;
         double x0 = 0;
         double x1 = 0;


         #region @ draw every month line
         foreach (var item in items)
         {
            if (item.TimeStamp.Month == month && item != items.Last())
               continue;

            month = item.TimeStamp.Month;
            x1 = this._state.OffsetX + this._state.BarWidth * items.IndexOf(item);
            Point p1 = new Point(x0, 0);
            Point p2 = new Point(x1, rect.Height);

            if (items.Last() == item)
               p2 = rect.BottomRight;

            Rect rectM = new Rect(p1, p2);
            if (month % 2 != 0)
               dc.DrawRectangle(Res.Month1BKBrush, null, rectM);
            else
               dc.DrawRectangle(Res.Month2BKBrush, null, rectM);
            x0 = x1;
         }
         #endregion


         //@ draw selectionBar

         var selections = this._selectionPlots.FindAll(temp => temp.Date >= items[0].TimeStamp
         && temp.Date <= items[items.Count-1].TimeStamp);


         dc.PushOpacity(0.3);
         foreach (var selection in selections)
         {
            index = ts.IndexOfByDate(selection.Date,Data.FindDirection.Forward) - ts.IndexOfByDate(this._state.VisibleStart, Data.FindDirection.Forward);
            double x = this._state.BarWidth * index;
            x += this._state.OffsetX;

            Rect selRect = new Rect(x, 0, this._state.BarWidth, rect.Height);
            dc.DrawRectangle(selection.Brush, null, selRect);
         }

         dc.Pop();

      }
   }
}

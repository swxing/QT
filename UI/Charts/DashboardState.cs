#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using QT.Data;

namespace QT.UI.Charts
{

   public class DashboardState
   {

      /// <summary>空的State，方便判斷用的。</summary>
      public static DashboardState Empty = new();

      public bool IsFixedDateLine { get; set; }

      /// <summary>股票代碼</summary>
      public string Symbol { get; init; } = "";
      
      
      /// <summary>Interval</summary>
      public BarInterval Interval { get; init; }



      /// <summary>當isFixedDateLines為True時，UI需要有一條線來固定日期。線繪在那部份，就是這個屬性決定的。
      /// 例如說，通常固定線要看的是今天的行情，為了版面最大化，我們希望線在右邊，此時可以將屬性設為0.8。
      ///0是最左邊，1是最右邊，0.5是中間，預設為中間。
      ///如果說要能使用者可以自行移動這條線，則要寫個Diagram控項，然後呢有個tag可以移，移完後再改這個屬性。
      /// </summary>
      public double FixedDateLinePosition { get; set; } = 0.9f;

      // 可視區間（這兩個決定要給圖表的資料範圍）
      /// <summary>可視區間開始</summary>
      public DateTime VisibleStart { get; set; }
      //public DateTime VisibleEnd { get; init; }


      DateTime? _selectedDate;
      public DateTime? SelectedDate
      { 
         get=> _selectedDate;
         set { 
            if(this._selectedDate == value)
               return;
            this._selectedDate = value;
            OnSelectionDateChanged(value ?? DateTime.MinValue);
         }
      }

      // 縮放與平移輔助（可選）
      //public double ZoomLevel { get; init; } = 1.0;      // >1 表示放大
      public double OffsetX { get; set; } = 0.0;      // 像素偏移或比例偏移

      bool _isDrag = false;
      public bool IsDrag 
      { get=> _isDrag;
         set
         {
            this.OnDragStarted();
            this._isDrag = value;
         }
      }

      public double BarWidth { get; set; } = 20.0; // 每根K棒的寬度（像素）


      /// <summary>選取的日期改變了。有這個事件，則Diagram可以可以針對這個事件進行單獨的Update,而不用做全部的繪制。</summary>
      public event EventHandler<DateTime>? SelectedDateChanged;

      /// <summary>當選取的個股變動。</summary>
      public event EventHandler<string>? SymbolChanged;

      public event EventHandler<BarInterval>? IntervalChanged;


      /// <summary>拖拉開始</summary>
      public event Action? DragStarted;

      
      /// <summary>拖拉結束</summary>
      public event Action? DragEnded;

      
      /// <summary>請求更新所有的Charts</summary>
      public event Action? RefreshChartsUiRequested;

      //發送事件的方法
      public void OnSelectionDateChanged(DateTime newDate)
      {
         SelectedDateChanged?.Invoke(this, newDate);
      }

      //dragStarted事件發送
      public void OnDragStarted()
      {
         DragStarted?.Invoke();
      }

      /// <summary>請求將Charts全部重繪</summary>
      public void RequestRefreshChartsUI()
      {
         RefreshChartsUiRequested?.Invoke(); 
      }

   }




}

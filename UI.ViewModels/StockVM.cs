using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace QT.UI.ViewModels
{
   // StockVM 繼承 INotifyPropertyChanged 介面
   public class StockVM : INotifyPropertyChanged
   {
      // ===============================================
      // 核心屬性
      // ===============================================

      private string _symbol=null!;
      public string Symbol
      {
         get => _symbol;
         set
         {
            if (_symbol != value)
            {
               _symbol = value;
               OnPropertyChanged();
            }
         }
      }

      // 為了展示 MVVM 實時更新，將 LastPrice 和 Volume 設置為可讀寫

      private decimal _lastPrice;
      public decimal LastPrice
      {
         get => _lastPrice;
         set
         {
            if (_lastPrice != value)
            {
               _lastPrice = value;
               OnPropertyChanged();
               // 💡 可以在這裡計算並更新其他依賴屬性，例如漲跌幅
               // OnPropertyChanged(nameof(ChangePercent)); 
            }
         }
      }

      private long _volume;
      public long Volume
      {
         get => _volume;
         set
         {
            if (_volume != value)
            {
               _volume = value;
               OnPropertyChanged();
            }
         }
      }

      // ===============================================
      // OHLC 屬性 (Bar Data)
      // ===============================================

      private decimal _open;
      public decimal Open
      {
         get => _open;
         set
         {
            if (_open != value) { _open = value; OnPropertyChanged(); }
         }
      }

      private decimal _high;
      public decimal High
      {
         get => _high;
         set
         {
            if (_high != value) { _high = value; OnPropertyChanged(); }
         }
      }

      private decimal _low;
      public decimal Low
      {
         get => _low;
         set
         {
            if (_low != value) { _low = value; OnPropertyChanged(); }
         }
      }

      private decimal _close;
      public decimal Close
      {
         get => _close;
         set
         {
            if (_close != value) { _close = value; OnPropertyChanged(); }
         }
      }


      // ===============================================
      // INotifyPropertyChanged 實作
      // ===============================================

      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// 觸發屬性變更通知的方法
      /// </summary>
      /// <param name="name">屬性名稱 (由 [CallerMemberName] 自動填寫)</param>
      protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
      }
   }
}

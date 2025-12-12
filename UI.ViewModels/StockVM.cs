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



      
      public string _name=null!;
      public string Name
      {
         get => _name;
         set
         {
            if (_name != value)
            {
               _name = value;
               OnPropertyChanged();
            }
         }
      }


      
      /// <summary>在設計模式時使用的。</summary>
      public static StockVM DesignInstance
      {
         get { 
            return new StockVM()
            {
               Symbol="3661",
               Name="世芯-KY",
               Price =520.5M,
               Volume=123456,
               Open=515.0M,
               High=525.0M,
               Low=510.0M,
               Close=518.0M,
               ChangeAmount=5.5f,
               ChangePercentage=0.013f
            };
         }
      }


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

      private decimal _price;
      public decimal Price
      {
         get => _price;
         set
         {
            if (_price != value)
            {
               _price = value;
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


      private float _chanegeAmount;
      
      /// <summary>漲跌金額</summary>
      public float ChangeAmount
      {
         get => _chanegeAmount;
         set
         {
            if (_chanegeAmount != value) { _chanegeAmount = value; OnPropertyChanged(); }
         }
      }

      private float _changePercentage;
      
      /// <summary>漲跌幅</summary>
      public float ChangePercentage
         {
         get => _changePercentage;
         set
         {
            if (_changePercentage != value) { _changePercentage = value; OnPropertyChanged(); }
         }
      }

      public string ChangePercentageString
      {
         get
         {
           
            return ChangePercentage.ToString("P2");
         }
      }


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

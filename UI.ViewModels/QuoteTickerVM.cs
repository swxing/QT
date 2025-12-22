using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using QT.Core;


namespace QT.UI.ViewModels
{


   /// <summary>a VM for QuoteTickerBarV</summary>
   public class QuoteTickerVM : INotifyPropertyChanged
   {


      private string _symbol = "";
      private string _name = "";
      private float _last;          /// 最新價
      private float _change;        /// 漲跌
      private float _changeRate;       /// 漲跌幅
      private long _volume;               /// 成交量(股數)
      private bool _isFlashing;              /// 用來觸發淡入淡出效果的旗標


      /// <summary>個股代號</summary>
      public string Symbol
      {
         get => _symbol;
         set { _symbol = value; OnPropertyChanged(); }
      }


      /// <summary>個股名稱</summary>
      public string Name
      {
         get => _name;
         set { _name = value; OnPropertyChanged(); }
      }


      /// <summary>最新的報價…</summary>
      public float Last
      {
         get => _last;
         set
         {
            if (Math.Abs(_last - value) > float.Epsilon)       //這一行是因為我們使用 float，所以要用誤差值來判斷是否相等
            {
               _last = value;
               OnPropertyChanged();
               TriggerFlash();
            }
         }
      }


      /// <summary>漲跌</summary>
      public float Change
      {
         get => _change;
         set { 
            _change = value; 
            OnPropertyChanged();

            if (Change > 0)
               PriceBrush = Res.UpBrush;
            else if (Change < 0)
               PriceBrush = Res.DownBrush;
            else
               PriceBrush = Brushes.White;
         }
      }

      public string ChangeB
      {
         get=> QT.Core.Helper.FormatByPrice(Change);
      }

      /// <summary>漲跌幅</summary>
      public float ChangeRate
      {
         get => _changeRate;
         set { _changeRate = value; OnPropertyChanged();
         }
      }


      public string ChangeRateB
      {
         get => ChangeRate.ToString("P2");

      }




      /// <summary>成交量(股數)</summary>
      public long Volume
      {
         get => _volume;
         set { _volume = value; OnPropertyChanged(); }
      }

      public string VolumeB
      {
         get=>$"{Volume:N0}張";

      }

      /// <summary>是否要淡入淡出效果</summary>
      public bool IsFlashing
      {
         get => _isFlashing;
         set { _isFlashing = value; OnPropertyChanged(); }
      }

      
      /// <summary>
      /// 當價格更新時呼叫，用來觸發淡入淡出
      /// </summary>
      private void TriggerFlash()
      {
         // 簡單做法：翻轉 bool，讓 DataTrigger 每次都重新執行動畫
         IsFlashing = !IsFlashing;
      }

      ///// <summary>用來更新報價的，並且會引發Last事件。只會引發一次…</summary>
      //public void UpdateQuote(float last, float change, float changeRate, long volume)
      //{
      //   this._last = last;
      //   this._change = change;
      //   this._changeRate = changeRate;
      //   this._volume = volume;

      //   //進行通知，只通知Last就好，其他的屬性不需要每次都通知
      //   OnPropertyChanged(nameof(Last));

      //}

      Brush _priceBrush = Brushes.Red;

      public System.Windows.Media.Brush PriceBrush
      {
         get => _priceBrush;
         
         set {

            if (this._priceBrush == value)
               return;
            this._priceBrush = value;

            OnPropertyChanged();
         }
      }

      public event PropertyChangedEventHandler? PropertyChanged;

      protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }


      /// <summary>在設計模式時使用的，方便呈現在設計階段的UI。</summary>
      public static QuoteTickerVM DesignInstance
      {
         get
         {
            return new QuoteTickerVM()
            {
               Symbol = "2330",
               Name = "台積電", 
               Last = 520.5f,
               Change = -5.5f,
               ChangeRate = 0.013f,
               Volume = 2355,
               
            };
         }
      }

   }



}

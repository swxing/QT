using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QT.UI.ViewModels
{


   /// <summary>給個股報價時用的VM</summary>
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


      /// <summary></summary>
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
         set { _change = value; OnPropertyChanged(); }
      }

      /// <summary>漲跌幅</summary>
      public float ChangeRate
      {
         get => _changeRate;
         set { _changeRate = value; OnPropertyChanged(); }
      }

      /// <summary>成交量(股數)</summary>
      public long Volume
      {
         get => _volume;
         set { _volume = value; OnPropertyChanged(); }
      }

      /// <summary>
      /// 讓 View 知道要做一次淡入淡出效果
      /// </summary>
      public bool IsFlashing
      {
         get => _isFlashing;
         private set { _isFlashing = value; OnPropertyChanged(); }
      }

      /// <summary>
      /// 當價格更新時呼叫，用來觸發淡入淡出
      /// </summary>
      private void TriggerFlash()
      {
         // 簡單做法：翻轉 bool，讓 DataTrigger 每次都重新執行動畫
         IsFlashing = !IsFlashing;
      }

      /// <summary>用來更新報價的。</summary>
      /// <param name="last"></param>
      /// <param name="change"></param>
      /// <param name="changeRate"></param>
      /// <param name="volume"></param>
      public void UpdateQuote(float last, float change, float changeRate, long volume)
      {
         Last = last;
         Change = change;
         ChangeRate = changeRate;
         Volume = volume;
      }

      public event PropertyChangedEventHandler? PropertyChanged;

      protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }


   }



}

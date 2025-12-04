using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace QT.UI.ViewModels
{
   public class StockFinderVM:System.ComponentModel.INotifyPropertyChanged
   {
      // 這是您在 XAML 中 ListView.ItemsSource 所綁定的集合
      public ObservableCollection<StockVM> QuotesCollection { get; set; }

      public StockFinderVM()
      {
         //載入所有的個股。
         QuotesCollection = new ObservableCollection<StockVM>();
        

       
      }


      // ===============================================
      // INotifyPropertyChanged 實作
      // ===============================================

      public event PropertyChangedEventHandler PropertyChanged;

      /// <summary>
      /// 觸發屬性變更通知的方法
      /// </summary>
      protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
      }
   }
}

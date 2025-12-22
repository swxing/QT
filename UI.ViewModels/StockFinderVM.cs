using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using QT.Data;

namespace QT.UI.ViewModels
{
   public class StockFinderVM:System.ComponentModel.INotifyPropertyChanged
   {
      // 這是您在 XAML 中 ListView.ItemsSource 所綁定的集合
      public ObservableCollection<StockVM> StockVMs { get; set; }

      public StockFinderVM()
      {
         //載入所有的個股。
         StockVMs = new ObservableCollection<StockVM>();

      }


      //建立一個股選取的屬性
      private StockVM? _selectedStockVM;
      public StockVM? SelectedStockVM
      {
         get => _selectedStockVM;
         set
         {
            if (_selectedStockVM != value)
            {
               _selectedStockVM = value;
               OnPropertyChanged();
            }
         }
      }


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

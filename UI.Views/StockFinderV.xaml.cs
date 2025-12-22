using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QT.UI.Views
{
   /// <summary>
   /// StockFinderV.xaml 的互動邏輯
   /// </summary>
   public partial class StockFinderV : UserControl
   {
      public StockFinderV()
      {
         InitializeComponent();
         this.livStocks.SelectionChanged += Stocks_SelectionChanged;  
      }

      private void Stocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         //當選取的個股發生改變時。
         var vm = this.DataContext as ViewModels.StockFinderVM
            ?? throw new InvalidOperationException("DataContext必須是StockFinderVM");

         vm.SelectedStockVM = this.livStocks.SelectedItem as QT.UI.ViewModels.StockVM;

      }


      /// <summary>容易取得資料模式</summary>
      private QT.UI.ViewModels.StockFinderVM VM
      {
         get
         {
            return this.DataContext as QT.UI.ViewModels.StockFinderVM
               ?? throw new InvalidOperationException("DataContext必須是StockFinderVM");
         }
      }


   }
}

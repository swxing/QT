using QT.UI.ViewModels;
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
   /// StockView.xaml 的互動邏輯
   /// </summary>
   public partial class StockV : UserControl
   {
      public StockV()
      {
         InitializeComponent();
      }


      StockVM _stockVm;
      public required StockVM CurrentQuote
      {
         get => _stockVm;
         set
         {
            if (_stockVm != value)
            {
               _stockVm = value;
               this.lblName.Content = this._stockVm.Symbol;          //先暫時性的
            }
         }
      }

   }
}

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
         this.DataContextChanged += StockV_DataContextChanged;
         this.Loaded += StockV_Loaded;
      }


      static Charts.ChartViewState _state = null;

      private void StockV_Loaded(object sender, RoutedEventArgs e)
      {
         
         if(_state==null)
         {
            _state = new Charts.ChartViewState()
            {
               Symbol = "3661",
               VisibleStart = DateTime.Now.AddMonths(-6),
               BarWidth = 20
            };

            
            

         }
            

      }

      private void StockV_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
      {
         
      //   this.CurrentQuote= this.DataContext as QT.UI.ViewModels.StockVM;
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
               //this.lblName.Content = "3661";
              ///this.lblName.Background = Brushes.AliceBlue;
            }
         }
      }

   }
}

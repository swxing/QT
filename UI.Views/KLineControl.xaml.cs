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
   /// KLineControl.xaml 的互動邏輯
   /// </summary>
   public partial class KLineControl : UserControl
   {


      Charts.ChartViewState _chartViewState;
      QT.UI.Charts.KLineChart _kLineChart;
            
      /// <summary>放indicatorWidget的Control</summary>
      StackPanel _stpIndicatorCtrs;


      /// <summary></summary>
      public required Charts.ChartViewState ChartViewState
      { 
         get { return _chartViewState; }
         set { _chartViewState = value;

            //建立建立ChartViewState，並設定給KLineChart
            _kLineChart = new Charts.KLineChart()
            {
               State = this.ChartViewState
            };

           grid.Children.Add(_kLineChart);

            //建立StackPanel，用來放指標控制項，放在Grid左上方(10,10)的位置
            var stackPanelIndicators = new StackPanel()
            {
               
               Width = 20,
               Height =20,
               Orientation = Orientation.Vertical,
               Margin = new Thickness(10, 10, 0, 0),
               HorizontalAlignment = HorizontalAlignment.Left,
               VerticalAlignment = VerticalAlignment.Top,
               Background= new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)) //半透明白色背景

            };

            grid.Children.Add(stackPanelIndicators);

         }
      }


      public KLineControl()
      {
         InitializeComponent();
      }

      public void AddIndicator(QT.Data.Indicators.Indicator indicator)
      { 
         this._kLineChart.AddIndicator(indicator);
         //建立Widget




      }




   }
}

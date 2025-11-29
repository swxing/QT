
using QT.Data;
using System.Windows;
using System.Windows.Controls;
using QT.UI;
using QT.UI.Charts;
namespace QT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

      private void btnTest_Click(object sender, RoutedEventArgs e)
      {

         this.CreateDashboard(); 
         ////////////測試用的
         //////////var State = new ChartViewState()
         //////////{
         //////////   Symbol = "3661",
         //////////   Interval = BarInterval.Day,
         //////////   VisibleStart = new DateTime(2025, 1, 1),
         //////////};

         //////////DashboardLid lid=new DashboardLid() { State = State };
         //////////BackgroundChart gkChart=new BackgroundChart() { State = State };
         //////////StateProcessor stateP = new () { State=State};
         //////////KLineChart kline = new () { State=State};
         //////////VolumnChart vChart=  new () { State=State};

         //////////Grid grid = new Grid();
         //////////grid.Children.Add(gkChart);
         //////////grid.Children.Add(kline);
         //////////grid.Children.Add(vChart);


         //////////grid.Children.Add(stateP);
         //////////grid.Children.Add(lid);


         //////////this.Content = grid;














      }//fn


      private void CreateDashboard()
      {
         //測試用的
         var State = new ChartViewState()
         {
            Symbol = "3661",
            Interval = BarInterval.Day,
            VisibleStart = new DateTime(2025, 1, 1),
         };


         Grid grid = new Grid();


         RowDefinition row1 = new();
         row1.Height = new GridLength(2, GridUnitType.Star);
         grid.RowDefinitions.Add(row1);

         var row2 = new RowDefinition();
         row2.Height = new GridLength(1, GridUnitType.Star);
         grid.RowDefinitions.Add(row2);

         var row3 = new RowDefinition();
         row3.Height = new GridLength(22, GridUnitType.Pixel);
         grid.RowDefinitions.Add(row3);



         grid.ShowGridLines = true;

         //加入底
         var bkChart = new BackgroundChart() { State = State };
         grid.Children.Add(bkChart);
         Grid.SetRow(bkChart, 0);
         Grid.SetRowSpan(bkChart, 3);

         //加入KLineChart
         var klineChart = new KLineChart() { State = State };
         grid.Children.Add(klineChart);

         //加入KLineInfoBarChart
         var infoBarChart = new KLineInfoBarChart() { State = State };
         grid.Children.Add(infoBarChart);

         //加入Volume
         var volumeChart = new VolumnChart() { State = State };
         grid.Children.Add(volumeChart);
         Grid.SetRow(volumeChart, 1);

         //加入日期軸
         var dateChart = new DateTimeChart() { State = State, Height = 22 };

         grid.Children.Add(dateChart);
         Grid.SetRow(dateChart, 2);


         //加上Dashboard Lid
         var dashboardLid = new DashboardLid() { State = State };
         dashboardLid.State = State;
         grid.Children.Add(dashboardLid);
         Grid.SetRowSpan(dashboardLid, 3);      //跨兩列

         //加入State Processor
         var stateProcessor = new ChartInteractionLayer() { State = State };
         grid.Children.Add(stateProcessor);
         Grid.SetRowSpan(stateProcessor, 3);   //跨三列
         this.Content = grid;






      }

   }//cls
}
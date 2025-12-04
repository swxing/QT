
using QT.Data;
using QT.UI;
using QT.UI.Charts;
using QT.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
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

      private async void btnTest_Click(object sender, RoutedEventArgs e)
      {
         

         this.CreateDashboard(); 
         //this.CreateSecurityQuoteBar();

      }//fn



      private async Task<List<Stock>> GetTwseOtcStocks()
      {

         var stocks = await QT.Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();
         var otc_stocks = await QT.Data.Api.OTC.OTCClient.Get上櫃公司基本資料Async();
         stocks.AddRange(otc_stocks);
         return stocks;

         ;






      }



      QuoteTickerVM CurrentQuote;

      private void CreateSecurityQuoteBar()
      {
         QT.UI.Views.QuoteTickerBarV barV = new();


         //建立一個VM
         QuoteTickerVM vm = new QuoteTickerVM()
         {
            Symbol = "2317",
            Name = "鴻海",
            Last = 102.5f,
            Change = 1.5f,
            ChangeRate = 0.014f,
            Volume = 98765432,
         };

         barV.DataContext = vm;

         this.gridMain.Children.Add(barV);
         return;

      }

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
         

         ////////加入指標
         //////Data.Indicators.SMAIndicator ma1= new Data.Indicators.SMAIndicator()
         //////{
         //////   Symbol = State.Symbol,
         //////   Interval = State.Interval,
         //////   Days = 20,
         //////   LineBrush = System.Windows.Media.Brushes.Orange,
         //////   LineWidth = 1.5,
         //////};

         //////Data.Indicators.SMAIndicator ma2 = new Data.Indicators.SMAIndicator()
         //////{
         //////   Symbol = State.Symbol,
         //////   Interval = State.Interval,
         //////   Days = 5,
         //////   LineBrush = System.Windows.Media.Brushes.White,
         //////   LineWidth = 3,
         //////};

         //////ma1.ReComputing();
         //////ma2.ReComputing();
         //////klineChart.AddIndicator(ma1);   
         //////klineChart.AddIndicator(ma2);

         grid.Children.Add(klineChart);


         //加入KLineInfoBarChart
         var infoBarChart = new KLineInfoBarChart() { State = State };
         grid.Children.Add(infoBarChart);

         //加入Volume
         var volumeChart = new VolumnChart() { State = State };
         grid.Children.Add(volumeChart);
         Grid.SetRow(volumeChart, 1);

         //加入日期軸
         //////var dateChart = new DateTimeChart() { State = State, Height = 22 };

         //////grid.Children.Add(dateChart);
         //////Grid.SetRow(dateChart, 2);


         ////////加上Dashboard Lid
         //////var dashboardLid = new DashboardLid() { State = State };
         //////dashboardLid.State = State;
         //////grid.Children.Add(dashboardLid);
         //////Grid.SetRowSpan(dashboardLid, 3);      //跨兩列

         //加入State Processor
         var stateProcessor = new ChartInteractionLayer() { State = State };
         grid.Children.Add(stateProcessor);
         Grid.SetRowSpan(stateProcessor, 3);   //跨三列

         this.gridMain.Children.Add(grid);






      }

      private void btnTestWindow_Click(object sender, RoutedEventArgs e)
      {
         單元測試Window w = new ();  
         w.ShowDialog();
      }

      private void btnTest2_Click(object sender, RoutedEventArgs e)
      {
         var price=System.Random.Shared.NextSingle();
            this.CurrentQuote.UpdateQuote(price, 2.0f, 0.36f, 23456789);
      }
   }//cls
}

using QT.Data;
using QT.UI;
using QT.UI.Charts;
using QT.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using QT.UI.Views;
namespace QT
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      ChartViewState _chartViewState;
      QT.UI.ViewModels.QuoteTickerVM _quoteTickerBarVM;
      StockFinderVM stockFinderVM;
      RevenueControl.RevenueVM _revenueVM;
      CalcIndexV.CalcIndexVM _calcIndexVM;               //統計項。


      public MainWindow()
      {
         InitializeComponent();

         //設定我們想要除錯的位置。如果只剩一下螢幕，則要注意。
         this.Width = 2000;
         this.Height = 1200;
         this.WindowStartupLocation = WindowStartupLocation.Manual;        //要切到手動，下面兩行才有效果
         this.Top = 100;
         this.Left = 2560;

         this.Loaded += MainWindow_Loaded;
      }

      private void MainWindow_Loaded(object sender, RoutedEventArgs e)
      {
         //要先建立State才行，有時想想…State不就Dashboard的VM嗎？
         this._chartViewState = new ChartViewState()
         {
            Symbol = "3661",
            Interval = BarInterval.Day,
            VisibleStart = new DateTime(2025, 11, 1),
         };

         this._chartViewState.SelectedDateChanged += _chartViewState_SelectedDateChanged;

         #region 建立StockFinderVM
         this.stockFinderVM= new StockFinderVM();
         stockFinderVM.PropertyChanged += StockFinderVM_PropertyChanged;
         var stocks = QT.Data.DataService.GetSysStockSet();

         //逐一加入StockVM
         foreach (var stock in stocks._securities)
         {
            StockVM stockVM = new StockVM
            {
               Symbol = stock.Symbol,
               Name = stock.ShortName,
               //取得最後一筆報價
               Open = 100,
               High = 105,
               Low = 99,
               Close = 102,
               Price = 102,
               Volume = 123456,
               ChangeAmount = 2,
               ChangePercentage = 0.019f
            };

            stockFinderVM.StockVMs.Add(stockVM);
         }

         this.stockFinder.DataContext = stockFinderVM;
         #endregion

         #region 建立TitleStock
         QT.UI.Views.QuoteTickerBarV _quoteTickerV = new();
         _quoteTickerBarVM = new QuoteTickerVM()
         {
            Symbol = "2317",
            Name = "鴻海",
            Last = 101.0f,
            Change = 1.0f,
            ChangeRate = 0.01f,
            Volume = 12345678
         };
         


         _quoteTickerV.DataContext = _quoteTickerBarVM;
         Grid.SetRow(_quoteTickerV, 0);
         this.gridChart.Children.Add(_quoteTickerV);
         #endregion
         
         #region Create Dashboard
         var dash_State = this.CreateDashboard(this._chartViewState);
         Grid.SetRow(dash_State, 1);
         this.gridChart.Children.Add(dash_State);
         


         #endregion

         #region ReveneControl
         this._revenueVM=new RevenueControl.RevenueVM() { Symbol=this._chartViewState.Symbol};
         QT.UI.Views.RevenueControl revenueControl = new() { ViewModel = this._revenueVM };
         grid2.Children.Add(revenueControl);


         #endregion


         #region CalcIndex
         _calcIndexVM = new CalcIndexV.CalcIndexVM();


         var calcIndexs = Indicators.CalcIndexes.GetCalcIndexes();

         //開始加入計算指標…
         foreach(var calc in calcIndexs)
         {
            _calcIndexVM.AddCalcIndex(calc);
         }



         CalcIndexV calcIndexV =new CalcIndexV() { DataContext=_calcIndexVM};
         grid2.Children.Add(calcIndexV);
         Grid.SetRow(calcIndexV, 1);

         #endregion

      }

      private void _chartViewState_SelectedDateChanged(object? sender, DateTime e)
      {
         //通知RevenueControl的日期
         this._revenueVM.UpdateSelectedDate(e);

         this._calcIndexVM.Calc(e);

      }

      private void StockFinderVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         //是否為選取個股改變
         if (e.PropertyName == nameof(StockFinderVM.SelectedStockVM))
         {
            var vm = sender as StockFinderVM;
            var selectedStockVM = vm?.SelectedStockVM;
            if(selectedStockVM == null)
               return;

            //更新ChartViewState
            this._chartViewState.Symbol = selectedStockVM.Symbol;
            this._chartViewState.RequestRefreshChartsUI();

            //更新RevenueVM
            this._revenueVM.Symbol=this._chartViewState.Symbol;               // vm會更新 DIs，RevenueV註冊vm的SymbolChanged，所以重載。

            //取得此股的最新報價資料(先用最後一筆測試)
            var bar =BarSet.GetBarSet(selectedStockVM.Symbol, BarInterval.Day).GetLatest();
            this._quoteTickerBarVM.Symbol = selectedStockVM.Symbol;          //報價Bar
            this._quoteTickerBarVM.Name = selectedStockVM.Name;
            this._quoteTickerBarVM.Last = bar!.Close;
            this._quoteTickerBarVM.Volume = bar.Volume;
            //隨機給個正/負數
            var Change = Random.Shared.Next(-100,100)/10.0f;
            this._quoteTickerBarVM.Change = Change;
            this._quoteTickerBarVM.ChangeRate = Change / bar.Close;

            //CalcIndex VM中，有些CalcIndex是關於"選取個股"的，所以也要通知他們
            this._calcIndexVM.SelectedSymbol= selectedStockVM.Symbol;


         }
      }//fn




      private async Task<List<Stock>> GetTwseOtcStocks()
      {

         var stocks = await QT.Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();
         var otc_stocks = await QT.Data.Api.OTC.OTCClient.Get上櫃公司基本資料Async();
         stocks.AddRange(otc_stocks);
         return stocks;

         ;






      }




      private Grid CreateDashboard(ChartViewState State)
      {



         Grid gridCharts = new Grid();
         RowDefinition row1 = new();
         row1.Height = new GridLength(2, GridUnitType.Star);
         gridCharts.RowDefinitions.Add(row1);

         var row2 = new RowDefinition();
         row2.Height = new GridLength(1, GridUnitType.Star);
         gridCharts.RowDefinitions.Add(row2);

         var row3 = new RowDefinition();
         row3.Height = new GridLength(22, GridUnitType.Pixel);
         gridCharts.RowDefinitions.Add(row3);



         gridCharts.ShowGridLines = true;

         //加入底
         var bkChart = new BackgroundChart() { State = State };
         gridCharts.Children.Add(bkChart);
         Grid.SetRow(bkChart, 0);
         Grid.SetRowSpan(bkChart, 3);

         //加入KLineChart
         //var klineChart = new KLineChart() { State = State };
         var klineChart = new QT.UI.Views.KLineControl() { ChartViewState=State };


         //加入指標
         Data.Indicators.SMAIndicator ma1 = new Data.Indicators.SMAIndicator()
         {
            Symbol = State.Symbol,
            Interval = State.Interval,
            Days = 20,
            LineBrush = System.Windows.Media.Brushes.Orange,
            LineWidth = 1.5,
         };

         Data.Indicators.SMAIndicator ma2 = new Data.Indicators.SMAIndicator()
         {
            Symbol = State.Symbol,
            Interval = State.Interval,
            Days = 5,
            LineBrush = System.Windows.Media.Brushes.White,
            LineWidth = 3,
         };

         ma1.ReComputing();
         ma2.ReComputing();
         klineChart.AddIndicator(ma1);
         klineChart.AddIndicator(ma2);

         gridCharts.Children.Add(klineChart);


         //加入KLineInfoBarChart
         var infoBarChart = new KLineInfoBarChart() { State = State };
         gridCharts.Children.Add(infoBarChart);

         //加入Volume
         var volumeChart = new VolumnChart() { State = State };
         gridCharts.Children.Add(volumeChart);
         Grid.SetRow(volumeChart, 1);

         //加入日期軸
         var dateChart = new DateTimeChart() { State = State, Height = 22 };

         gridCharts.Children.Add(dateChart);
         Grid.SetRow(dateChart, 2);


         //加上Dashboard Lid
         var dashboardLid = new DashboardLid() { State = State };
         dashboardLid.State = State;
         gridCharts.Children.Add(dashboardLid);
         Grid.SetRowSpan(dashboardLid, 3);      //跨兩列

         //加入State Processor
         var stateProcessor = new ChartInteractionLayer() { State = State };
         gridCharts.Children.Add(stateProcessor);
         Grid.SetRowSpan(stateProcessor, 3);   //跨三列

         gridCharts.ShowGridLines = false;

         return gridCharts;






      }

      private void btnTestWindow_Click(object sender, RoutedEventArgs e)
      {
         單元測試Window w = new();
         w.ShowDialog();
      }


   }//cls
}
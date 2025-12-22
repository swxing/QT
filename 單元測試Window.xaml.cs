
using Microsoft.Extensions.DependencyInjection;
using QT.Data;
using QT.Data.Api;
using System.Windows.Controls;
using System.Windows;




namespace QT
{
   /// <summary>
   /// 單元測試Window.xaml 的互動邏輯 
   /// </summary>
   public partial class 單元測試Window : Window
   {
      public 單元測試Window()
      {
         InitializeComponent();
      }





      private async void btnCnYesGetBars_Click(object sender, RoutedEventArgs e)
      {

         var stocks=QT.Data.DataService.GetSysStockSet();

         //顯示是否要進行下載
         MessageBoxResult result= MessageBox.Show($"是否要下載股價資料, 共{stocks._securities.Count}檔？","下載確認",MessageBoxButton.YesNo,MessageBoxImage.Question);
         if(result!= MessageBoxResult.Yes)
            return;

         //逐一下載
         foreach(var sec in stocks._securities)
         {
            var bars = await CnYes.GetBarsAsync(sec.Symbol, Data.BarInterval.Day, new DateTime(2010, 11, 1), new DateTime(2025, 12, 15));
            DataService.UpsertManyBar(bars!);
         }



         //var bars =await CnYes.GetBarsAsync("3661", Data.BarInterval.Day, new DateTime(2010, 11, 1), new DateTime(2025, 12, 13));
         //DataService.UpsertManyBar(bars!);
         

      }



      private async void btnGetSecurity_Click(object sender, RoutedEventArgs e)
      {

         var stocks = DataService.GetSysStockSet();
         DataGrid gg = new DataGrid();
         gg.ItemsSource=stocks._securities;  
         this.grid.Children.Clear();
         this.grid.Children.Add(gg);
         return;

         //////var securities = await QT.Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();
         //////var securitiesOTC = await QT.Data.Api.OTC.OTCClient.Get上櫃公司基本資料Async();
         //////securities.AddRange(securitiesOTC);
         //////this.dg.ItemsSource = securities;   
         var securitys=DataService.GetSecuritySets();
         var dg = new System.Windows.Controls.DataGrid();
         dg.ItemsSource = securitys;
         this.grid.Children.Clear();
         this.grid.Children.Add(dg);


      }

      private async void btnUpdateSecuritysToDB_Click(object sender, RoutedEventArgs e)
      {
         MessageBox.Show("程式碼已註解");
         return;
         var stocks =await  QT.Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();
         var stocksOTC = await QT.Data.Api.OTC.OTCClient.Get上櫃公司基本資料Async();
         stocks.AddRange(stocksOTC);
         DataService.UpsertManyStocks(stocks);
      }

      private void 加入單一個股與資料(Stock stock)
      {





      }




      private void btnAddSecuritySet_Click(object sender, RoutedEventArgs e)
      {

         //var ss= DataService.GetSecuritySets();
         //;


         
         //return;








         //get security sets 
         var sets= DataService.GetSecuritySets();

         var alls = DataService.GetSysStockSet();




         //add a new security set
         StockSet set = new StockSet()
         {
            Name="我的自選股",
            Description="這是我的自選股清單",
         };

         set.Add(alls.FindBySymbol("3661")!);
         set.Add(alls.FindBySymbol("5388")!);

         //save to db
         DataService.UpsertSecuritySet(set);
      }



      private void btnStockFinder_Click(object sender, RoutedEventArgs e)
      {
         QT.UI.Views.StockFinderV finderV = new UI.Views.StockFinderV();


         //建立ViewModel
         var sysSecuritys= DataService.GetSysStockSet();
         var finderVM = new UI.ViewModels.StockFinderVM();

         foreach(var sec in sysSecuritys._securities)
         {
            var stockVM = new UI.ViewModels.StockVM()
            {
               Symbol = sec.Symbol,
               Name = sec.ShortName,
               Price = 100,
               Volume = 1000,
               Open = 99,
               High = 101,
               Low = 98,
               Close = 100,
               ChangeAmount = 1.5f,
               ChangePercentage = 0.015f,

            };


            finderVM.StockVMs.Add(stockVM);
         }


         finderV.DataContext = finderVM;

         this.grid.Children.Clear(); 
         this.grid.Children.Add(finderV);






      }

      private async void SaveEPS()
      {



         var stocks = DataService.GetSysStockSet();
         foreach (var sec in stocks._securities)
         {
            string url = $"https://marketinfo.api.cnyes.com/mi/api/v1/TWS:{sec}:STOCK/eps";
            string json = await QT.Data.Api.Helper.GetResponseAsync(url);

            //解析json
            var eps = await CnYes.GetEPSAsync(sec.Symbol);
            DataService.SaveEpsRange(eps);


         }
      }

      private async void btnGetEPS_Click(object sender, RoutedEventArgs e)
      {

         var epss= await CnYes.GetEPSAsync("3661");
         DataGrid gg = new DataGrid();
         gg.ItemsSource = epss;
         this.grid.Children.Clear();
         this.grid.Children.Add(gg);

      }

      private async void btnRevenue_Click(object sender, RoutedEventArgs e)
      {

        //var stocks = DataService.GetSysStockSet();
        // foreach (var stock in stocks._securities)
        // {
        //    var revenues = await Data.Api.CnYes.GetRevenuesAsync(stock.Symbol);
        //    DataService.SaveRevenueRange(revenues);
        // }

        // return;




         var revFromDb = DataService.GetRevenueList("2330");
         DataGrid gg = new DataGrid();

         gg.ItemsSource = revFromDb;
         this.grid.Children.Clear();
         this.grid.Children.Add(gg);

      }

      private void Test2()
      {
         //string url = @"https://ws.api.cnyes.com/ws/api/v1/charting/history?resolution=D&symbol=TWS:3661:STOCK&from=1388534400&to=1262304000&quote=0";
         //var json = QT.Data.Api.Helper.GetResponseAsync(url).Result;

         //取得twse股票列表
         var stocks = Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();

      }


      private async void btnAddStock_Click(object sender, RoutedEventArgs e)
      {


         var stocks = new List<Stock>()
{
       new Stock { Symbol = "TSE01", Name = "台灣加權指數", ShortName = "台灣加權指數", PublishType = StockPublishType.TWSE },
    new Stock { Symbol = "0050", Name = "元大台灣50", ShortName = "元大台灣50", PublishType = StockPublishType.TWSE },
    new Stock { Symbol = "0056", Name = "元大高股息", ShortName = "元大高股息", PublishType = StockPublishType.TWSE },
    new Stock { Symbol = "00981A", Name = "主動統一台股增長", ShortName = "主動統一台股增長", PublishType = StockPublishType.TWSE }, // 請確認此代碼名稱
    new Stock { Symbol = "00919", Name = "群益台灣精選高股息", ShortName = "群益台灣精選高股息", PublishType = StockPublishType.TWSE },
    new Stock { Symbol = "00878", Name = "國泰永續高股息", ShortName = "國泰永續高股息", PublishType = StockPublishType.TWSE },
    new Stock { Symbol = "00713", Name = "元大台灣高息低波", ShortName = "元大台灣高息低波", PublishType = StockPublishType.TWSE }
};

         DataService.UpsertManyStocks(stocks);
         foreach (var stock in stocks)
         {

            //取得bars
            var bars = await CnYes.GetBarsAsync(stock.Symbol, Data.BarInterval.Day, new DateTime(2010, 1, 1), new DateTime(2025, 12, 19));

            // bars不等於null, 存入db
            if (bars != null)
               DataService.UpsertManyBar(bars);

            //取得eps
            var epss = await CnYes.GetEPSAsync(stock.Symbol);
            DataService.SaveEpsRange(epss);

            //取得營收
            var revenues = await CnYes.GetRevenuesAsync(stock.Symbol);
            DataService.SaveRevenueRange(revenues);
         }
      }

      
      private async void  btn股息_Click(object sender, RoutedEventArgs e)
      {
         var ds = DataService.GetDividendList("2330");
         //呈現在grid
         DataGrid dg = new DataGrid() { ItemsSource = ds };
         this.grid.Children.Add(dg);
         return;


         var sysStocks = DataService.GetSysStockSet();
         foreach(var stock in sysStocks._securities)
         {
            var dividends = await Data.Api.CnYes.GetDividendAsync(stock.Symbol);
            DataService.SaveDividendRange(dividends);
            //var ds = DataService.GetDividendList("00713");
            //呈現在grid
            //DataGrid dg = new DataGrid() { ItemsSource = ds };
            //this.grid.Children.Add(dg);
         }
         
      }

      private void btn統計項_Click(object sender, RoutedEventArgs e)
      {
         // 1. 建立 View 的實例
         QT.UI.Views.CalcIndexV calcV = new QT.UI.Views.CalcIndexV();

         // 2. 建立 Window
         Window window = new Window();

         // 3. 關鍵修正：將 calcV 設為 Window 的「內容 (Content)」，它才會顯示在畫面上
         window.Content = calcV;

         // 4. 設定視窗屬性 (選用)
         window.Title = "量化指標統計";
         window.Width = 400;
         window.Height = 600;

         // 5. 顯示視窗
         window.Show();

      }
   }//cls
}//ns

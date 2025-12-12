
using Microsoft.Extensions.DependencyInjection;
using QT.Data;
using QT.Data.API;
using QT.Data.Repos;
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


         var bars=await CnYes.GetBarsAsync("3661", Data.BarInterval.Day, new DateTime(2015, 11, 1), new DateTime(2025, 11, 20));
         DataService.UpsertManyBar(bars!);
         

      }



      private async void btnGetSecurity_Click(object sender, RoutedEventArgs e)
      {
         //////var securities = await QT.Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();
         //////var securitiesOTC = await QT.Data.Api.OTC.OTCClient.Get上櫃公司基本資料Async();
         //////securities.AddRange(securitiesOTC);
         //////this.dg.ItemsSource = securities;   
         var securitys=DataService.GetSecuritySets();
         var dg = new System.Windows.Controls.DataGrid();
         dg.ItemsSource = securitys;
         this.gdContent.Children.Clear();
         this.gdContent.Children.Add(dg);


      }

      private async void btnUpdateSecuritysToDB_Click(object sender, RoutedEventArgs e)
      {
         await DataService.UpdateOnlineSecuritysToDB();
         
      }

      private void btnAddSecuritySet_Click(object sender, RoutedEventArgs e)
      {

         //var ss= DataService.GetSecuritySets();
         //;


         
         //return;








         //get security sets 
         var sets= DataService.GetSecuritySets();

         var alls = DataService.GetSysSecuritySet();




         //add a new security set
         SecuritySet set = new SecuritySet()
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
         var sysSecuritys= DataService.GetSysSecuritySet();
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

         this.gdContent.Children.Clear(); 
         this.gdContent.Children.Add(finderV);






      }


   }//cls
}//ns

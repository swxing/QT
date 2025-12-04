
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

         var repo = new QT.Data.Repos.BarRepository();
         repo.UpsertMany(bars!);
         return;

      }



      private async void btnGetSecurity_Click(object sender, RoutedEventArgs e)
      {
         //////var securities = await QT.Data.Api.TWSE.TwseTools.Get上市公司基本資料Async();
         //////var securitiesOTC = await QT.Data.Api.OTC.OTCClient.Get上櫃公司基本資料Async();
         //////securities.AddRange(securitiesOTC);
         //////this.dg.ItemsSource = securities;   
         var securitys=DataService.GetAllSecuritysFromDB();
         this.dg.ItemsSource = securitys;


      }

      private async void btnUpdateSecuritysToDB_Click(object sender, RoutedEventArgs e)
      {
         await DataService.UpdateSecurityToDB();
         
      }
   }
}

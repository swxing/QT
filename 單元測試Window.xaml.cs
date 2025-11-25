
using Microsoft.Extensions.DependencyInjection;
using QT.Data;
using QT.Data.API;
using QT.Data.Repository;
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
         

         //var bars=await CnYes.GetBarsAsync("3661", Data.BarInterval.Day, new DateTime(2015, 11, 1), new DateTime(2025, 11, 20));
         //repo.Upsert(bars!);

         var barSet=DataService.GetBarSet("3661", Data.BarInterval.Day);
         //var bars=repo.GetAll("3661", Data.BarInterval.Day);

         var bars=barSet.GetRange(new DateTime(2025, 1, 1), new DateTime(2025, 11, 1));
         var  dt=new DateTime(2025, 10, 25);  
         var bar=barSet.FindBar(dt);
         var bar1 = barSet.FindBar(dt, FindDirection.Forward);
         var bar2 = barSet.FindBar(dt, FindDirection.Forward,-3);
         var bar3 = barSet.FindBar(dt, FindDirection.Forward, 3);

         var bar4 = barSet.FindBar(dt, FindDirection.Backward, -3);
         var bar5 = barSet.FindBar(dt, FindDirection.Backward, 3);


         dg.ItemsSource = bars;


      }
    }
}

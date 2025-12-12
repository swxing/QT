using LiteDB;
using QT.Data;

using System.Configuration;
using System.Data;
using System.Windows;
//微軟推薦的寫作風格，相依性注入，需要安裝 NuGet 套件 Microsoft.Extensions.DependencyInjection
//也就是把要用到的物件都註冊到服務集合(Service Collection)中，然後在需要的地方建構子注入(Constructor Injection)使用
using Microsoft.Extensions.DependencyInjection;   

namespace QT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

      // 靜態屬性，讓整個應用程式都可以取得注入的服務
      public static IServiceProvider Services { get; private set; } = null!;

      protected override async void OnStartup(StartupEventArgs e)
      {

         #region 相依性注入服務註冊
         //////////var services = new ServiceCollection();

         ////////////以下三行是註冊服務到服務集合中，系統會自動處理相依性注入，就會new出需要的物件, 
         ////////////例如 第二行BarRepository 需要 db才能new出來，系統會自動把第一行的 LiteDatabase 注入到 BarRepository 的建構子中
         ////////////所以順序也很重要，必須先註冊 LiteDatabase 才能註冊 BarRepository
         ////////////如何才能在app中使用注入的物件呢？例如要在 MainWindow 使用 BarRepository
         //////////// app.services.GetService<BarRepository>() 就可以取得注入的物件
         //////////services.AddSingleton<LiteDatabase>(_ => new LiteDatabase("qt.db"));
         //////////services.AddSingleton<BarRepository>();            
         
         ////////////把服務集合建構成服務提供者，然後存到靜態屬性中，讓整個應用程式都可以取得注入的服務
         
         //////////Services = services
         //////////    .BuildServiceProvider(new ServiceProviderOptions
         //////////    {
         //////////       //兩行，讓容器支援屬性注入與驗證（未來會用得到）
         //////////       ValidateScopes = true,
         //////////       ValidateOnBuild = true   // 啟動時立刻檢查所有依賴能不能解析，錯立刻爆
         //////////    });
         #endregion



         base.OnStartup(e);

      }

   }

}


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
         Dashboard board = new ()
         {

            //測試用的
            State = new DashboardState()
            {
               Symbol = "3661",
               Interval = BarInterval.Day,
               VisibleStart = new DateTime(2025, 1, 1),
            }

         };

         this.Content = board;
         board.InitializeDashboard();

      }//fn


    }//cls
}
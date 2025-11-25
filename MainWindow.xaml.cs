using QT.Control;
using QT.Data;
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
         Control.Dashboard board = new Control.Dashboard()
         {

            //測試用的
            State = new DashboardState()
            {
               Symbol = "3661",
               Interval = BarInterval.Day,
               ZoomLevel = 1.0,
               VisibleStart = new DateTime(2025, 1, 1),
            }

         };

         //Window win = new();
         //win.Content = board;
         //board.InitializeDashboard();
         
         //win.ShowDialog();
         //return;


         this.Content = board;
         board.InitializeDashboard();


      }//fn





    }//cls
}
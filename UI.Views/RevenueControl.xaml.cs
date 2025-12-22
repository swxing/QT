using System.Windows;
using System.Windows.Controls;
using QT.Data;

namespace QT.UI.Views
{
   /// <summary></summary>
   public partial class RevenueControl : UserControl
   {

      public RevenueControl()
      {
         InitializeComponent();
         this.Loaded += RevenueControl_Loaded;
      }

      private void RevenueControl_Loaded(object sender, RoutedEventArgs e)
      {
         //@ 建立style
         var rowStyle = QT.UI.DataGridHelper.DataGridRowStyle1;
         this.dg.RowStyle = rowStyle;
         this.dg.AutoGenerateColumns = false;

         ColumnInfo infoDate = new ColumnInfo("年月", "Date", typeof(DateTime), "yyyy-MM");
         ColumnInfo infoRevenue = new ColumnInfo("本期營收", "Revenue", typeof(float), "N0");
         ColumnInfo infoRevenueAccumulated = new ColumnInfo("今年營收", "RevenueAccumulated", typeof(float), "N0");
         ColumnInfo infoYoY = new ColumnInfo("年增率", "YoY", typeof(float), "P0");
         ColumnInfo infoYoYAccumulated = new ColumnInfo("年增率\r\n(累計)", "YoYAccumulated", typeof(float), "P0");
         infoRevenue.TextAlignment = TextAlignment.Right;
         infoRevenueAccumulated.TextAlignment = TextAlignment.Right;
         infoYoY.TextAlignment = TextAlignment.Right;
         infoYoYAccumulated.TextAlignment = TextAlignment.Right;

         List<ColumnInfo> infos = new List<ColumnInfo>();
         infos.Add(infoDate);
         infos.Add(infoYoY);                                         //年增率較重要，所以排在前面。
         infos.Add(infoYoYAccumulated);
         infos.Add(infoRevenue);
         infos.Add(infoRevenueAccumulated);

         foreach (ColumnInfo info in infos)
         {
            var col = QT.UI.DataGridHelper.CreateDataGridTextColumn(info);
            col.IsReadOnly = true;
            this.dg.Columns.Add(col);
         }


      }

      RevenueVM _vm;
      public RevenueVM ViewModel
      {
         get => _vm;
         set
         {
            _vm = value;
            this._vm.SelectedRevenueChanged += _vm_SelectedRevenueChanged;
            this._vm.SymbolChanged += _vm_SymbolChanged;
            this.dg.ItemsSource = _vm._allRevenueDIs;
         }


      }

      private void _vm_SymbolChanged()
      {
         this.dg.ItemsSource = null;
         this.dg.ItemsSource = _vm._allRevenueDIs;
      }

      private void _vm_SelectedRevenueChanged(RevenueDI revenue)
      {
         this.dg.SelectedItem = revenue;                  //選取DataGrid的對應列
         if (revenue == null)
            return;
         this.dg.ScrollIntoView(revenue);                 //捲動到該列
      }



      /// <summary>只是RevenueItem的包裏一層成為RowDI</summary>
      public class RevenueDI : RowDI
      {
         Revenue _item = null;
         internal RevenueDI(Revenue item)
         {
            _item = item;
         }

         public float Revenue { get => _item.Amount; }
         public float RevenueAccumulated { get => _item.AmountAccumulated; }

         public float YoY { get => _item.YoY; }

         public float YoYAccumulated { get => _item.YoYAccumulated; }
         public DateTime Date { get => _item.Date; }
      }


      /// <summary>提供RevenueV的VM。</summary>
      public class RevenueVM
      {
         string _symbol;
         public event Action<RevenueDI> SelectedRevenueChanged;
         public event Action SymbolChanged;

         //internal List<Revenue> _allRevenues = null;          //某檔的所有的營收。

         public List<RevenueDI> _allRevenueDIs = null;    //某檔的所有營收的DI。>

         /// <summary> 目前選取的營收資料</summary>
         private RevenueDI _selectedRevenue = null;

         /// <summary>個股的代號</summary>
         public required string Symbol
         {
            get => _symbol;

            set
            {
               if (_symbol == value) return;
               _symbol = value;
               var allRevenues = DataService.GetRevenueList(_symbol);         //取得營收資料。

               //將Revenue轉成RevenueDI
               _allRevenueDIs = new List<RevenueDI>();
               foreach (var rev in allRevenues)
               {
                  var di = new RevenueDI(rev);
                  if (di.Date.Month == 12)
                     di.DataLayer = 3;
                  else
                     di.DataLayer = 2;
                  _allRevenueDIs.Add(di);
               }
               this.OnSymbolChanged();
            }
         }


         //當使用者選取某個月份的營收時，觸發事件
         public void OnSelectedRevenueChanged(RevenueDI revenueDI)
         {
            SelectedRevenueChanged?.Invoke(revenueDI);
         }

         public void OnSymbolChanged()
         {
            SymbolChanged?.Invoke();
         }

         /// <summary>給外界設定的選取日</summary>
         public void UpdateSelectedDate(DateTime date)
         {
            var dt1 = new DateTime(date.Year, date.Month, 1);                 //對應到該月的1日
            var revenueDI = _allRevenueDIs?.Find(r => r.Date == dt1);             //找到對應的營收資料
            if (revenueDI != _selectedRevenue)
            {
               _selectedRevenue = revenueDI;
               OnSelectedRevenueChanged(_selectedRevenue);        //觸發事件通知外部
            }
         }
      }


   }//cls
}//ns

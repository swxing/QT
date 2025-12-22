using QT.Indicators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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


namespace QT.UI.Views
{
   /// <summary>
   /// CalcIndexV.xaml 的互動邏輯
   /// </summary>
   public partial class CalcIndexV : UserControl      //控制項
   {

      public CalcIndexV()
      {
         InitializeComponent();
      }

      

      private CalcIndexVM? VM
      {
         get => this.DataContext as CalcIndexVM;
      }

      /*
       巢狀類。VM與DI
       
       */


      /// <summary>
      /// 顯示項 (Display Item)，專供 DataGrid 繫結使用。
      /// </summary>
      public class DI : INotifyPropertyChanged
      {


         private string _name=string.Empty;
         private string _value = string.Empty;
         private string _info = string.Empty;

         public string Name
         {
            get => _name;
            set {
               if (value == Name) return;

               _name = value; OnPropertyChanged(); }
         }

         public string Value
         {
            get => _value;
            set {
               if (value == _value) return;
               _value = value; OnPropertyChanged(); }
         }

         public string Info
         {
            get => _info;
            set { 
               if(value == _info)    return;
               _info = value; OnPropertyChanged(); }
         }

         // --- INotifyPropertyChanged 實作 ---

         public event PropertyChangedEventHandler? PropertyChanged;

         /// <summary>
         /// 使用 CallerMemberName 會自動抓取呼叫者的屬性名稱
         /// </summary>
         protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
         {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }



















      }


      /// <summary>為統計控制項的VM，控制項中有一個VM。
      /// 控制項會取得裡面的DIS，
      /// </summary>
      public class CalcIndexVM
      {
         //會含有多個CalcIndex。然後每個再對應一個DI。
         //而系統則是對照到這個DI列。
         Dictionary<QT.Indicators.CalcIndex, DI> _calcsDIs=new Dictionary<CalcIndex, DI>();

         /// <summary>這是繫結專用的。</summary>
         private ObservableCollection<DI> _displayItems = new();
         public ObservableCollection<DI> DisplayItems { get => _displayItems; }

         public CalcIndexVM()
         {


         }



         string _selectedSymbol = string.Empty; 
         /// <summary>CalcIndex中，有一些是個"選取個股"相關的，此時選取的個股會由外部利用此屬性傳入。然後
         /// 再進行設定。…</summary>
         public string SelectedSymbol
         { 
            get=> _selectedSymbol;
            set
            {
               if (value == _selectedSymbol) return;
               _selectedSymbol = value;

               //找出所有CalcIndex，看看有沒有符合的。
               foreach (var index in _calcsDIs.Keys.OfType<漲跌幅Index>().Where(i => i.IsFixedSymbol))
               {
                  index.Symbol = value;      //設定Symbol。
               }
            }

         }


         public void AddCalcIndex(CalcIndex calcIndex)
         {
            
            DI di = new DI();
            di.Name =calcIndex.Name;
            _calcsDIs.Add(calcIndex, di);              //一個計算，一個呈現。
            _displayItems.Add(di);
         }

         public void RemoveCalcIndex(CalcIndex calcIndex)
         {
            var pair = _calcsDIs[calcIndex];
            _displayItems.Remove(pair);
            _calcsDIs.Remove(calcIndex);
         }

         public void Calc(DateTime dt)
         {

            foreach (var pair in _calcsDIs)
            {
               var result=pair.Key.Calc(dt);
               pair.Value.Value = result.value;
               pair.Value.Info = result.info;
            }



         }






      }//class



   }//cls

}//ns




using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace QT
{



   public static class Res
   {


      /// <summary></summary>
      /// <param name="status"></param>
      /// <returns></returns>
      public static Brush GetBrush(QT.Data.PriceChangeStatus status)
      {
         return status == Data.PriceChangeStatus.Up ? Res.UpBrush :
                status == Data.PriceChangeStatus.Down ? Res.DownBrush :
                Brushes.Gray;
      }


      //建立漲的Pen
      public static readonly Pen UpPen = new Pen(Brushes.Red, 1.0);
      public static readonly Brush UpBrush = Brushes.Red;
      
      //建立跌的Pen
      public static readonly Pen DownPen = new Pen(Brushes.Green, 1.0);
      public static readonly Brush DownBrush = Brushes.Green;

      //建立平盤的Pen
      public static readonly Pen FlatPen = new Pen(Brushes.White, 1.0);
      public static readonly Brush FlatBrush = Brushes.White;

      public static Pen VerLinePen = new Pen(Brushes.White, 1);
      
      public static Pen DiaBorPen = new Pen(Brushes.Black, 3);
      public static Brush DiaBkBrush = new SolidColorBrush(Color.FromArgb(150, 51, 51, 51));          //Dialog的背色
      
      public static Brush DashboardBkBrush = new SolidColorBrush(Color.FromArgb(255, 31, 31, 31));    //dashboard的背色
      
      /// <summary>KBar的選取色</summary>
      public static Brush SelectionBarBrush = Brushes.AliceBlue;

      //虛線樣式
      private static List<double> crossPenDash = new List<double>() { 8, 4 };            //8為線，10為空
      public static Pen CrossLinePen = new Pen(Brushes.White, 1) { DashStyle = new DashStyle(crossPenDash, 1) };


      //月份交替背色
      public static Brush Month1BKBrush = new SolidColorBrush(Color.FromArgb(255, 31, 31, 31));    //dashboard的背色
      public static Brush Month2BKBrush = new SolidColorBrush(Color.FromArgb(255, 37, 37, 37));    //dashboard的背色








   }//cls
}//ns

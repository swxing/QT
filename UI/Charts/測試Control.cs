using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace QT.UI.Charts
{
    public class 測試Control:System.Windows.Controls.Control
    {

      public 測試Control()
      {
         this.Width = 300;
         this.Height = 200;
         this.Background = System.Windows.Media.Brushes.AliceBlue;

         


      }
      protected override void OnRender(DrawingContext dc)
      {
         base.OnRender(dc);

         // 畫滿整個控制項為黑色
         dc.DrawRectangle(
             Brushes.Black,
             null,
             new Rect(0, 0, ActualWidth, ActualHeight)
         );
      }


   }
}

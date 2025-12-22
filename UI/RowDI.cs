using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QT.UI
{




   /// <summary>用來實作一個資料Row要呈現的元素。可以實做 Visibility。
   ///如果要實做Row的背景色，或是Font大小等，請設定DataGrid.RowStyle，
   ///亦有現成的Style可取：DataGridRowStyle1; </summary>
   //[Serializable]
   public class RowDI : System.ComponentModel.INotifyPropertyChanged
   {

      /// <summary>用來儲存轉譯的訊息，當isTranslation為True時，會採用這裡的字樣。</summary>
      Dictionary<string, string> _dicts = null;

      /// <summary>當啟用代理服務時，則值會跟介面取。</summary>
      public bool IsTranslation { get; set; } = false;

      int _dataLayer = 0;

      public event PropertyChangedEventHandler PropertyChanged;


      protected void OnPropertyChanged(string name)
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(name));
      }

      /// <summary>資料的層級，最低階為0, 1，2等依此類推。而RowStyle會針對DataLayer進行設計</summary>
      public int DataLayer
      {
         get { return this._dataLayer; }
         set { this._dataLayer = value; }
      }


      /// <summary>取得轉譯的值。</summary>
      public string GetTranslation(string propName)
      {
         if (this._dicts == null)
            return "";
         if (this._dicts.ContainsKey(propName) == false)
            return "";

         return this._dicts[propName];
      }

      /// <summary>實作一整列資料出現的邏輯。</summary>
      public virtual Visibility Visibility { get; set; } = Visibility.Visible;

      /// <summary>設定轉譯的值</summary>
      public void SetTranslation(string propName, string value)
      {

         if (this._dicts == null)
            this._dicts = new Dictionary<string, string>();
         this._dicts.Add(propName, value);

      }


      Brush _background = null;
      /// <summary>當Background為null時，則會採用DataLayer的規範。否則採用Background</summary>
      public Brush Background
      {
         get => _background;
         set
         {

            _background = value;
            this.OnPropertyChanged("Background");

         }
      }

   }



}

using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NewTask.ViewModels
{
    [Table]
    public class ItemViewModel : INotifyPropertyChanged
    {
        [Column(DbType="INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        //private string _id;
        ///// <summary>
        ///// Sample ViewModel property; this property is used to identify the object.
        ///// </summary>
        ///// <returns></returns>
        //public string ID
        //{
        //    get
        //    {
        //        return _id;
        //    }
        //    set
        //    {
        //        if (value != _id)
        //        {
        //            _id = value;
        //            NotifyPropertyChanged("ID");
        //        }
        //    }
        //}

        
        private string _lineOne;

        [Column(CanBeNull = false)]
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _lineTwo;

        [Column]
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _lineThree;

        [Column]
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                if (value != _lineThree)
                {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        // Deleting the item from the collection and database
        
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.Devices.Geolocation;

namespace MyTask
{
    public class TaskItem : INotifyPropertyChanged
    {
        public TaskItem() { }
        
        /// <summary>
        /// Constructor called if there's no location information available
        /// </summary>
        /// <param name="dateTimeParameter"></param>
        /// <param name="details"></param>
        public TaskItem(DateTime dateTimeParameter, string title, string details)
        {
            this.dateTime = dateTimeParameter;
            this.Title = title;
            this.Details = details;

            // add a position
            longitude = (App.Current as App).GeoLocator.currentPosition.Coordinate.Longitude.ToString();
            latitude = (App.Current as App).GeoLocator.currentPosition.Coordinate.Latitude.ToString();

            createUniqueIdentity();
        }

        /// <summary>
        /// All-encompassing constructor
        /// </summary>
        /// <param name="dateTimeParameter"></param>
        /// <param name="details"></param>
        /// <param name="Position"></param>
        public TaskItem(DateTime dateTimeParameter, string details, Geoposition Position)
        {
            this.dateTime = dateTimeParameter;
            this.Details = details;


            createUniqueIdentity();
        }


        /// <summary>
        /// This is created using the dateTime string
        /// </summary>
        private string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }
        }

        /// <summary>
        /// Geoposition object to contain more than just longitude & latitude.
        /// UPDATE: can't serialize the whole object
        /// </summary>
        //public Geoposition location { get; set; }

        public string longitude { get; set; }
        public string latitude { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public DateTime dateTime { get; set; }


        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("title");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private string _details;
        public string Details
        {
            get { return _details; }
            set
            {
                if (_details != value)
                {
                    _details = value;
                    NotifyPropertyChanged("details");
                }
            }
        }

        public void createUniqueIdentity()
        {
            this._id = dateTime.Year.ToString() + dateTime.DayOfYear.ToString() + dateTime.Hour.ToString() + dateTime.Minute.ToString() + dateTime.Second.ToString() + dateTime.Millisecond.ToString();
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MyTask.Tools
{
    public class GPS
    {
        private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;

        private string errorMessage;
        public string longitude;
        public string latitude;
        public string status;

        public Geoposition currentPosition;
        public Geopoint currentPoint;

        public GPS()
        {
            _geolocator = new Geolocator();

#if WINDOWS_PHONE_APP
            // You must set the MovementThreshold for 
            // distance-based tracking or ReportInterval for
            // periodic-based tracking before adding event handlers.
            //
            // Value of 1000 milliseconds (1 second)
            // isn't a requirement, it is just an example.
            _geolocator.ReportInterval = 1000;
#endif

            if (currentPosition == null)
            {
                while (currentPosition == null)
                {
                    _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
                    _geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);
                }
                // update the strings for quick viewing
                if (currentPosition != null)
                {
                    longitude = currentPosition.Coordinate.Longitude.ToString();
                    latitude = currentPosition.Coordinate.Latitude.ToString();
                }
            }

            //StartTracking();

        }

        /// <summary>
        /// Single location request
        /// </summary>
        /// <returns></returns>
        public async Task GetGeoLocation()
        {
            try
            {
                // Get cancellation token
                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                // Get position
                currentPosition = await _geolocator.GetGeopositionAsync().AsTask(token);

                longitude = currentPosition.Coordinate.Longitude.ToString();
                latitude = currentPosition.Coordinate.Latitude.ToString();

            }
            catch (System.UnauthorizedAccessException e)
            {
                errorMessage = e.Message;
            }
            catch(TaskCanceledException e)
            {
                errorMessage = e.Message;
            }
            finally
            {
                _cts = null;
            }

        }

        /// <summary>
        /// Begin continuous tracking
        /// </summary>
        private void StartTracking()
        {
            _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            _geolocator.StatusChanged += new TypedEventHandler<Geolocator, StatusChangedEventArgs>(OnStatusChanged);

            // update the strings for quick viewing
            if (currentPosition != null)
            {
                longitude = currentPosition.Coordinate.Longitude.ToString();
                latitude = currentPosition.Coordinate.Latitude.ToString();
            }
        }

        /// <summary>
        /// This is the event handler for PositionChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            Geoposition pos = e.Position;
            currentPosition = pos;

            //rootPage.NotifyUser("Updated", NotifyType.StatusMessage);
            /*
            ScenarioOutput_Latitude.Text = pos.Coordinate.Point.Position.Latitude.ToString();
            ScenarioOutput_Longitude.Text = pos.Coordinate.Point.Position.Longitude.ToString();
            ScenarioOutput_Accuracy.Text = pos.Coordinate.Accuracy.ToString(); */
        }


        /// <summary>
        /// This is the event handler for StatusChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case PositionStatus.Ready:
                    // Location platform is providing valid data.
                    status = "Ready";
                    break;

                case PositionStatus.Initializing:
                    // Location platform is acquiring a fix. It may or may not have data. Or the data may be less accurate.
                    status = "Initializing";
                    break;

                case PositionStatus.NoData:
                    // Location platform could not obtain location data.
                    status = "No data";
                    break;

                case PositionStatus.Disabled:
                    // The permission to access location data is denied by the user or other policies.
                    status = "Disabled";

                    //Clear cached location data if any
                    status = "No data";
                    break;

                case PositionStatus.NotInitialized:
                    // The location platform is not initialized. This indicates that the application has not made a request for location data.
                    status = "Not initialized";
                    break;

                case PositionStatus.NotAvailable:
                    // The location platform is not available on this version of the OS.
                    status = "Not available";
                    break;

                default:
                    status = "Unknown";
                    break;
            }
        }

    }
}

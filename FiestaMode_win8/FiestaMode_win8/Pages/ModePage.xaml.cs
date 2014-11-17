using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FiestaMode.Converters;
using Windows.Phone.UI.Input;

namespace FiestaMode.Pages
{

    public partial class ModePage : Page
    {
        
        protected enum ARDUINO_MODE { NONE, PLAYER_COLORS, AMBIENT_COLORS, FIESTA, TIMER, RAINBOW, POKER };

        protected static int[] LUT = new int[] {
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  1,
            1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,
            2,  3,  3,  3,  3,  3,  3,  3,  4,  4,  4,  4,  4,  5,  5,  5,
            5,  6,  6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9, 10,
           10, 10, 11, 11, 11, 12, 12, 13, 13, 13, 14, 14, 15, 15, 16, 16,
           17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 24, 24, 25,
           25, 26, 27, 27, 28, 29, 29, 30, 31, 32, 32, 33, 34, 35, 35, 36,
           37, 38, 39, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 50,
           51, 52, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 66, 67, 68,
           69, 70, 72, 73, 74, 75, 77, 78, 79, 81, 82, 83, 85, 86, 87, 89,
           90, 92, 93, 95, 96, 98, 99,101,102,104,105,107,109,110,112,114,
          115,117,119,120,122,124,126,127,129,131,133,135,137,138,140,142,
          144,146,148,150,152,154,156,158,160,162,164,167,169,171,173,175,
          177,180,182,184,186,189,191,193,196,198,200,203,205,208,210,213,
          215,218,220,223,225,228,231,233,236,239,241,244,247,249,252,255 
        };

        private Uri _nextTask = null;

        private bool isNextTaskReady = false;

        protected static bool _isUpdatingArduino = false;
        protected static bool IsUpdatingArduino
        {
            get
            {
                return _isUpdatingArduino;
            }
            set
            {
                _isUpdatingArduino = value;
                UpdateProgressIndicator();
            }
        }

        protected static bool _failedToUpdateArduino = false;
        protected static bool FailedToUpdateArduino
        {
            get
            {
                return _failedToUpdateArduino;
            }
            set
            {
                _failedToUpdateArduino = value;
                UpdateProgressIndicator();
            }
        }

        protected const string ARDUINO_BASE_URL = "arduino";

        public ModePage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected string ColorToGammaShiftedHex( Color color )
        {
            Color col = new Color();

            //Lookup color shift in gamma table to correct for color and brightness innaccuracy
            col.R = (byte)LUT[(int)color.R];
            col.G = (byte)LUT[(int)color.G];
            col.B = (byte)LUT[(int)color.B];

            string hex = col.ToHex();
            hex = hex.Substring(3); //remove # and alpha, argb order
            hex = hex.ToLower(); //lowercase

            return hex;
        }

        protected async void SetNextUpdateTask( string urlArgs )
        {
            if (!urlArgs.EndsWith("/"))
                urlArgs += "/";
            Uri url = new Uri( "http://" + App.ViewModel.IPAddress + "/" + ARDUINO_BASE_URL + "/" + urlArgs);

            Debug.WriteLine("Adding url " + url.ToString() );

            _nextTask = url;

            if (!isNextTaskReady)
            {
                isNextTaskReady = true;
                string result = await UpdateArduinoAsync();
            }
        }

        protected async Task<string> UpdateArduinoAsync()
        {
            await Task.Run(() =>
            {
                while (IsUpdatingArduino)
                { }
            });

            if (!isNextTaskReady)
                return "";
            isNextTaskReady = false;

            FailedToUpdateArduino = false;
            IsUpdatingArduino = true;

            bool failed = false;
            string responseStr = null;
            try
            {
                HttpWebRequest req = HttpWebRequest.CreateHttp(_nextTask);
                WebResponse response = await req.GetResponseAsync();
                Stream respStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(respStream);
                responseStr = reader.ReadToEnd();

                Debug.WriteLine("Arduino Updated Successfully with result: " + responseStr);
            }
            catch (WebException e)
            {
                Debug.WriteLine("Error updating Arduino!");

                failed = true;
            }

            
            IsUpdatingArduino = false;
            FailedToUpdateArduino = failed;
            

            return responseStr;
        }

        private static void UpdateProgressIndicator()
        {
            Debug.WriteLine("Updating progress indicator...");

            if (_failedToUpdateArduino)
            {
                App.ViewModel.ProgressIndicator.Text = "Error contacting Arduino";
                App.ViewModel.ProgressIndicator.ShowAsync();
            }
            else if (_isUpdatingArduino)
            {
                App.ViewModel.ProgressIndicator.Text = "Updating Arduino...";
                App.ViewModel.ProgressIndicator.ShowAsync();
            }
            else
                App.ViewModel.ProgressIndicator.HideAsync();
        }

        protected void OnArduinoUpdated(IAsyncResult result)
        {
            bool failed = false;
            HttpWebRequest req = (HttpWebRequest)result.AsyncState;
            if (req != null)
            {
                try 
                {
                    HttpWebResponse resp = (HttpWebResponse)req.EndGetResponse( result );

                    Stream respStream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(respStream);
                    string responseStr = reader.ReadToEnd();

                    Debug.WriteLine("Arduino Updated Successfully with result: " + responseStr);
                }
                catch (WebException e)
                {
                    Debug.WriteLine("Error updating Arduino!");

                    failed = true;
                }
            }

            IsUpdatingArduino = false;
            FailedToUpdateArduino = failed;
        }


        public virtual void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

    }
}

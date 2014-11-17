using FiestaMode.Pages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

namespace FiestaMode.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public string IPAddress { get; set; }

        private StatusBarProgressIndicator progressIndicator;
        public StatusBarProgressIndicator ProgressIndicator
        {
            get
            {
                return this.progressIndicator;
            }
            set
            {
                if (value == this.progressIndicator)
                {
                    return;
                }

                this.progressIndicator = value;
                this.NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            //Console.Out.WriteLine(this.Items.Count);
            this.Items = new ObservableCollection<ItemViewModel>();

            this.IPAddress = "192.168.0.140";
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Initialize singleton Progress Indicator
            StatusBarProgressIndicator indicator = StatusBar.GetForCurrentView().ProgressIndicator;
            indicator.Text = "";
            App.ViewModel.ProgressIndicator = indicator;

            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { 
                PageType = typeof(ModePage_SingleColor), 
                LineOne = "Single Color", 
                LineTwo = "Single Solid Color",
                Image = new BitmapImage(new Uri("ms-appx:///Images/rgb.png"))
            });

            this.Items.Add(new ItemViewModel() { 
                PageType = typeof(ModePage_PlayerColor), 
                LineOne = "Player Color", 
                LineTwo = "Color per player",
                Image = new BitmapImage(new Uri("ms-appx:///Images/rgb.png"))
            });

            /*this.Items.Add(new ItemViewModel() { 
                PageURL = new Uri(@"/Pages/ModePage_Ambiance.xaml", UriKind.Relative), 
                LineOne = "Ambiance", 
                LineTwo = "Smooth transitions between colors", 
                ImageURL = new Uri(@"/Images/rgb.png", UriKind.Relative) 
            });*/
            this.Items.Add(new ItemViewModel() { 
                PageType = typeof(ModePage_Rainbow), 
                LineOne = "Rainbow", 
                LineTwo = "Rotating rainbow hues",
                Image = new BitmapImage(new Uri("ms-appx:///Images/rgb.png"))
            });

            this.Items.Add(new ItemViewModel() { 
                PageType = typeof(ModePage_Poker), 
                LineOne = "Poker", 
                LineTwo = "Color per player and cycle dealer",
                Image = new BitmapImage(new Uri("ms-appx:///Images/rgb.png"))
            });
           
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
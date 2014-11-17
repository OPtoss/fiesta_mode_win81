using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI;

namespace FiestaMode.ViewModels
{
    public class AmbianceModel : INotifyPropertyChanged
    {
        public AmbianceModel()
        {
            this.AmbianceColors = new ObservableCollection<ColorItem>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ColorItem> AmbianceColors { get; set; }

        public float AmbianceSpeed { get; set; }

        public float AmbianceScale { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads default values for this model
        /// </summary>
        public void LoadData()
        {
            AmbianceSpeed = 0.5f;
            AmbianceScale = 0.5f;

            AmbianceColors.Add(new ColorItem() { Text = "1", Color = Colors.Yellow });
            AmbianceColors.Add(new ColorItem() { Text = "2", Color = Colors.Green });
            AmbianceColors.Add(new ColorItem() { Text = "3", Color = Colors.Blue });
            AmbianceColors.Add(new ColorItem() { Text = "4", Color = Colors.Red });
            
            IsDataLoaded = true;
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
    }
}
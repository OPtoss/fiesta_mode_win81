using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FiestaMode.ViewModels
{
    public class RainbowModel : INotifyPropertyChanged
    {
        public RainbowModel()
        {

        }

        public float RainbowSpeed { get; set; }

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
            RainbowSpeed = 0.5f;

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
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI;

namespace FiestaMode.ViewModels
{
    public class PlayerColorsModel : INotifyPropertyChanged
    {
        public PlayerColorsModel()
        {
            this.AllColors = new ObservableCollection<ColorItem>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ColorItem> AllColors { get; set; }

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
            // Sample data; replace with real data
            this.AllColors.Add(new ColorItem() { Text = "1", Color = Colors.Yellow });
            this.AllColors.Add(new ColorItem() { Text = "2", Color = Colors.Green });
            this.AllColors.Add(new ColorItem() { Text = "3", Color = Colors.Blue });
            this.AllColors.Add(new ColorItem() { Text = "4", Color = Colors.Purple });
            this.AllColors.Add(new ColorItem() { Text = "5", Color = Colors.Magenta });
            this.AllColors.Add(new ColorItem() { Text = "6", Color = Colors.Red });
            this.AllColors.Add(new ColorItem() { Text = "7", Color = Colors.Orange });
            this.AllColors.Add(new ColorItem() { Text = "8", Color = Colors.Gray });
            
            this.IsDataLoaded = true;
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
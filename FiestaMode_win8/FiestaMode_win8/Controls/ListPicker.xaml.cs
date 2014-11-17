using FiestaMode.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FiestaMode.Controls
{
    public sealed partial class ListPicker : UserControl
    {
        private PickerItem _selectedItem;
        public PickerItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
        }

        public Thickness SpacerHeight { get; private set; }

        private List<PickerItem> pickerItems;

        public ListPicker()
        {
            this.InitializeComponent();

            this.DataContext = this;

            scroller.ViewChanging += scroller_ViewChanging;

            float spacer = 550f / 2f - 100f / 2f + 1;
            SpacerHeight = new Thickness( 0, spacer, 0, spacer );
        }

        void UpdateSelected( double scrollPos )
        {
            int index = (int)((scrollPos - SpacerHeight.Top) / 100f);

            for (int i = 0; i < itemsControl.Items.Count; ++i)
            {
                PickerItem item = (PickerItem)itemsControl.Items[i];

                ContentPresenter presenter = (ContentPresenter)itemsControl.ContainerFromItem(item);
                Grid grid = VisualTreeHelper.GetChild(presenter, 0) as Grid;
                var transform = grid.TransformToVisual(this);
                Point absolutePosition = transform.TransformPoint(new Point(0, 0));

                if (absolutePosition.Y < SpacerHeight.Top + 50 && absolutePosition.Y > SpacerHeight.Top - 50)
                {
                    grid.Background = new SolidColorBrush(Colors.Green);
                    _selectedItem = item;
                }
                else
                {
                    grid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
            }
        }

        void scroller_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            UpdateSelected(e.FinalView.VerticalOffset);
        }

        

        public void SetItems( List<object> items, string label="" )
        {
            pickerItems = new List<PickerItem>();

            for (int i = 0; i < items.Count; ++i)
            {
                PickerItem item = new PickerItem(items[i].ToString(), label);
                pickerItems.Add(item);
            }
            scroller.ChangeView(0, SpacerHeight.Top, 1);

            itemsControl.ItemsSource = pickerItems;

            Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => { UpdateSelected(SpacerHeight.Top); });
            
        }

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var transform = (sender as UIElement).TransformToVisual(this);
            Point absolutePosition = transform.TransformPoint(new Point(0, 0));

            scroller.ChangeView( null, absolutePosition.Y, null, false );

            UpdateSelected(scroller.VerticalOffset);
        }
    }
}

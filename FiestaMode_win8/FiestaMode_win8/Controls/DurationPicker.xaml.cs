using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FiestaMode.Controls
{
    public sealed partial class DurationPicker : UserControl
    {

        private const float LIST_HEIGHT = 550f;
        private const float ITEM_HEIGHT = 90f;
        private const float SPACER_HEIGHT = (550f / 2f) - (90f / 2f);

        public TimeSpan Duration
        {
            get
            {
                int hours = int.Parse(hoursList.SelectedItem.Text);
                int minutes = int.Parse(minutesList.SelectedItem.Text);
                int seconds = int.Parse(secondsList.SelectedItem.Text );
                return new TimeSpan(hours, minutes, seconds);
            }
        }

        public DurationPicker()
        {
            this.InitializeComponent();

            BuildStack(hoursList, 24, "hours");
            BuildStack(minutesList, 60, "minutes");
            BuildStack(secondsList, 60, "seconds");
        }

        private void BuildStack(ListPicker list, int maxValue, string label)
        {
            List<object> items = new List<object>();
            for (int i = 0; i < maxValue; ++i)
                items.Add((object)i.ToString());

            list.SetItems(items, label);
        }


    }

    
}

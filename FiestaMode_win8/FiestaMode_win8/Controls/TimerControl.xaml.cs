using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FiestaMode.Controls
{
    public partial class TimerControl : Grid
    {
        public delegate void StartTimerEventHandler(object sender, StartTimerEventArgs e);

        public event StartTimerEventHandler StartTimer;

        private TimeSpan _timerDuration;
        public TimeSpan TimerDuration
        {
            get
            {
                return _timerDuration;
            }
            set
            {
                _timerDuration = value;
                durationButton.Content = _timerDuration.ToString(@"hh\:mm\:ss");
            }
        }

        public TimerControl()
        {
            InitializeComponent();

            TimerDuration = new TimeSpan();
        }

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartTimer != null)
            {
                int milli = (int)TimerDuration.TotalMilliseconds;
                StartTimer(this, new StartTimerEventArgs(sender, milli));
            }
        }

        private void durationButton_Click(object sender, RoutedEventArgs e)
        {
            var pf = new PickerFlyout();
            var durationPicker = new DurationPicker();
            pf.Closed += DurationPicker_Closed;
            pf.Content = durationPicker;
            pf.ConfirmationButtonsVisible = true;
            pf.ShowAt(durationButton);
        }

        void DurationPicker_Closed(object sender, object e)
        {
            DurationPicker picker = ((PickerFlyout)sender).Content as DurationPicker;
            TimerDuration = picker.Duration;
        }
    }

    public class StartTimerEventArgs : RoutedEventArgs
    {
        private int milliseconds;

        public int Milliseconds
        {
            get { return milliseconds; }
        }

        public StartTimerEventArgs(object sender, int milli)
            : base()
        {
            this.milliseconds = milli;
        }
    }
}

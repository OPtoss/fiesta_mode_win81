using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Diagnostics;
using FiestaMode.ViewModels;
using FiestaMode.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;

namespace FiestaMode.Pages
{
    public partial class ModePage_SingleColor : ModePage
    {
        public Color _currentColor;

        public Color CurrentColor
        {
            get
            {
                return _currentColor;
            }
            set
            {
                _currentColor = value;
                colorButton.Background = new SolidColorBrush(_currentColor);
            }
        }

        private ColorPickerPage _colorPicker;

        public ModePage_SingleColor() : base()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            CurrentColor = Colors.Blue;

            Loaded += ModePage_SingleColor_Loaded;
        }

        void ModePage_SingleColor_Loaded(object sender, RoutedEventArgs e)
        {
            DoUpdateArduino();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorPickerPage page = new ColorPickerPage();
            
            Frame.Navigate(typeof(ColorPickerPage), null);
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _colorPicker = e.Content as ColorPickerPage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_colorPicker != null && _colorPicker.SelectedColorItem != null)
            {
                Debug.WriteLine("Got color " + _colorPicker.SelectedColorItem.Text);
                CurrentColor = _colorPicker.SelectedColorItem.Color;

                DoUpdateArduino();
            }
        }

        protected void DoUpdateArduino()
        {
            List<string> args = new List<string>();
            args.Add(((int)ARDUINO_MODE.PLAYER_COLORS).ToString());
            for (int i = 0; i < 8; ++i)
            {
                args.Add(ColorToGammaShiftedHex(CurrentColor));
            }
            string urlArgs = string.Join("/", args);
            SetNextUpdateTask(urlArgs);
        }

        private void fiestaButton_Click(object sender, RoutedEventArgs e)
        {
            SetNextUpdateTask(((int)ARDUINO_MODE.FIESTA).ToString());
        }

        private void onStartTimer_Clicked(object sender, StartTimerEventArgs e)
        {
            List<string> args = new List<string>();
            args.Add(((int)ARDUINO_MODE.TIMER).ToString());
            args.Add(e.Milliseconds.ToString());
            string urlArgs = string.Join("/", args);
            SetNextUpdateTask(urlArgs);
        }
    }
}
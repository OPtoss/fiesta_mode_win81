using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using FiestaMode.ViewModels;
using System.Threading;
using FiestaMode.Controls;
using FiestaMode.Pages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Input;

namespace FiestaMode
{
    public partial class ModePage_Ambiance : ModePage
    {

        private ColorPickerPage _colorPicker;

        private ColorItem _lastClickedItem;

        private AmbianceModel _model;

        public AmbianceModel ViewModel
        {
            get
            {
                if (_model == null)
                    _model = new AmbianceModel();
                return _model;
            }
        }

        public ModePage_Ambiance() : base()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            //LayoutRoot.DataContext = ViewModel;

            Loaded += ModePage_PlayerColor_Loaded;
        }

        void ModePage_PlayerColor_Loaded(object sender, RoutedEventArgs e)
        {
            DoUpdateArduino();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //clear selection
                //listBox.SelectedItem = null;

                _lastClickedItem = ((ColorItem)e.AddedItems[0]);

                Debug.WriteLine(sender);
                Debug.WriteLine(_lastClickedItem);

                ColorPickerPage page = new ColorPickerPage();
                Frame.Navigate(typeof(ColorPickerPage), null);
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _colorPicker = e.Content as ColorPickerPage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!_model.IsDataLoaded)
            {
                _model.LoadData();
            }

            if (_colorPicker != null && _colorPicker.SelectedColorItem != null)
            {
                Debug.WriteLine("Got color " + _colorPicker.SelectedColorItem.Text);
                _lastClickedItem.Color = _colorPicker.SelectedColorItem.Color;
                //_lastClickedItem.Text = _lastClickedItem.Color.ToString();

                DoUpdateArduino();
            }
        }

        protected void DoUpdateArduino()
        {
            List<string> args = new List<string>();
            //mode
            args.Add( ((int)ARDUINO_MODE.AMBIENT_COLORS).ToString());
            //speed
            args.Add(ViewModel.AmbianceSpeed.ToString());
            //scale
            args.Add(ViewModel.AmbianceScale.ToString());
            //colors
            for (int i = 0; i < ViewModel.AmbianceColors.Count; ++i)
            {
                args.Add(ColorToGammaShiftedHex(ViewModel.AmbianceColors[i].Color));
            }
            string urlArgs = string.Join("/", args);
            SetNextUpdateTask(urlArgs);
        }

       /* private void speedSlider_ValueChangedd(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ViewModel.AmbianceSpeed = (float)e.NewValue;
        }

        private void speedSlider_ManipulationCompletedd(object sender, ManipulationCompletedEventArgs e)
        {
            Timer t = new Timer( OnTimerTick, this, 500, 0 );
        }

        private void scaleSlider_ValueChangedd(object sender, object e)
        {
            ViewModel.AmbianceScale = (float)e.NewValue;
        }

        private void scaleSlider_ManipulationCompletedd(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            Timer t = new Timer(OnTimerTick, this, 500, 0);
        }
        */


        private void OnTimerTick(object state)
        {
            DoUpdateArduino();
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
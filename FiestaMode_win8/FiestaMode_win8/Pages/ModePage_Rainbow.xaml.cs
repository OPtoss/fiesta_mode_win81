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
using Windows.UI.Xaml.Controls.Primitives;

namespace FiestaMode
{
    public partial class ModePage_Rainbow : ModePage
    {
        private RainbowModel _model;

        public RainbowModel ViewModel
        {
            get
            {
                if (_model == null)
                    _model = new RainbowModel();
                return _model;
            }
        }

        public ModePage_Rainbow()
            : base()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            LayoutRoot.DataContext = ViewModel;

            Loaded += ModePage_PlayerColor_Loaded;
        }

        void ModePage_PlayerColor_Loaded(object sender, RoutedEventArgs e)
        {
            DoUpdateArduino();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!_model.IsDataLoaded)
            {
                _model.LoadData();
            }

            DoUpdateArduino();
        }

        protected void DoUpdateArduino()
        {
            List<string> args = new List<string>();
            //mode
            args.Add( ((int)ARDUINO_MODE.RAINBOW).ToString());
            //speed
            args.Add(ViewModel.RainbowSpeed.ToString());
            string urlArgs = string.Join("/", args);
            SetNextUpdateTask(urlArgs);
        }

        private void speedSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ViewModel.RainbowSpeed = (float)e.NewValue;
        }

        private void speedSlider_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            Timer t = new Timer( OnTimerTick, this, 500, 0 );
        }


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
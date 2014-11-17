using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using FiestaMode.ViewModels;
using FiestaMode.Controls;
using Windows.UI.Xaml;
using FiestaMode.Pages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FiestaMode
{
    public partial class ModePage_PlayerColor : ModePage
    {

        private ColorPickerPage _colorPicker;

        private ColorItem _lastClickedItem;

        private PlayerColorsModel _model;

        public PlayerColorsModel ViewModel
        {
            get
            {
                if (_model == null)
                    _model = new PlayerColorsModel();
                return _model;
            }
        }

        public ModePage_PlayerColor() : base()
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
            _colorPicker = e.Content as ColorPickerPage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!ViewModel.IsDataLoaded)
            {
                ViewModel.LoadData();
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
            args.Add(((int)ARDUINO_MODE.PLAYER_COLORS).ToString());
            for (int i = 0; i < ViewModel.AllColors.Count; ++i)
            {
                args.Add(ColorToGammaShiftedHex(ViewModel.AllColors[i].Color));
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

        private void Color_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.DataContext is ColorItem)
            {
                ColorItem item = (ColorItem)element.DataContext;

                _lastClickedItem = item;

                ColorPickerPage page = new ColorPickerPage();
                Frame.Navigate(typeof(ColorPickerPage), null);
            }
        }

    }
}
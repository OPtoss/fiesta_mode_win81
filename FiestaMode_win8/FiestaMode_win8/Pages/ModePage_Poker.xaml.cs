using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using FiestaMode.ViewModels;
using FiestaMode.Pages;
using FiestaMode.Controls;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.Phone.UI.Input;

namespace FiestaMode
{
    public partial class ModePage_Poker : ModePage
    {

        private ColorPickerPage _colorPicker;

        private ColorItem _lastClickedItem;

        private PokerModel _model;

        private DispatcherTimer _blindTimer;
        private TimeSpan _blindTimerLength;

        private DateTime _blindTimerPauseTime;
        private DateTime _blindTimerStartTime;

        private Random random;

        public PokerModel ViewModel
        {
            get
            {
                if (_model == null)
                    _model = new PokerModel();
                return _model;
            }
        }

        public ModePage_Poker()
            : base()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            LayoutRoot.DataContext = ViewModel;

            Loaded += ModePage_Poker_Loaded;

            random = new Random((int)DateTime.Now.TimeOfDay.Seconds);

            cycleButton.Content = "Start";

            //disable blind timer til game starts
            startBlindTimerButton.IsEnabled = false;
            blindTimerInput.IsEnabled = false;
            resetBlindTimerButton.IsEnabled = false;
        }

        

        void ModePage_Poker_Loaded(object sender, RoutedEventArgs e)
        {
            DoUpdateArduino();
        }

        private void PokerPlayer_Tap(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.DataContext is PokerPlayer)
            {
                PokerPlayer player = (PokerPlayer)element.DataContext;

                _lastClickedItem = player;

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
            if (!ViewModel.IsDataLoaded)
            {
                ViewModel.LoadData();
            }

            if (_colorPicker != null && _colorPicker.SelectedColorItem != null)
            {
                Debug.WriteLine("Got color " + _colorPicker.SelectedColorItem.Text);
                _lastClickedItem.Color = _colorPicker.SelectedColorItem.Color;

                DoUpdateArduino();
            }
        }


        protected void DoUpdateArduino()
        {
            List<string> args = new List<string>();
            args.Add(((int)ARDUINO_MODE.PLAYER_COLORS).ToString());
            for (int i = 0; i < ViewModel.Players.Count; ++i)
            {
                // TODO rework this mode to not use PlayerMode and send dealer/small/big separately
                Color color = ViewModel.Players[i].PokerColor;
                if (ViewModel.Players[i].IsInGame)
                {
                    if (ViewModel.Players[i] == PokerPlayer.Dealer)
                    {
                        color = Colors.Red;
                    }
                    else if (ViewModel.Players[i] == PokerPlayer.SmallBlind)
                    {
                        color = Colors.Green;
                    }
                    else if (ViewModel.Players[i] == PokerPlayer.BigBlind)
                    {
                        color = Colors.Blue;
                    }
                }
                args.Add(ColorToGammaShiftedHex(color));
            }
            //args.Add(ViewModel.DealerIndex.ToString());
            //args.Add(ViewModel.SmallIndex.ToString());
            //args.Add(ViewModel.BigIndex.ToString());
            string urlArgs = string.Join("/", args);
            SetNextUpdateTask(urlArgs);
        }

        private void cycleButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)cycleButton.Content == "Start")
            {
                cycleButton.Content = "Cycle";

                //enable blind timer
                startBlindTimerButton.IsEnabled = true;
                blindTimerInput.IsEnabled = true;
                resetBlindTimerButton.IsEnabled = true;

                //randomize starting dealer
                ViewModel.DealerActiveIndex = random.Next(0, ViewModel.Players.Count-1);
            }

            ViewModel.CycleDealer();

            DoUpdateArduino();
        }

        private void PokerPlayer_Hold(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == Windows.UI.Input.HoldingState.Started)
            { 
                FrameworkElement element = (FrameworkElement)e.OriginalSource;
                if (element.DataContext is PokerPlayer)
                {
                    PokerPlayer player = (PokerPlayer)element.DataContext;
                    Debug.WriteLine("Hold on player " + player.Text);

                    ViewModel.TogglePlayerInGame(player);

                    DoUpdateArduino();
                }
                else
                {
                    Debug.WriteLine("Hold on list but couldn't find player");
                    Debug.WriteLine( element.DataContext );
                }
            }
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

        private void startBlindTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)startBlindTimerButton.Content == "Start")
            {
                startBlindTimerButton.Content = "Pause";

                _blindTimerLength = TimeSpan.FromMinutes(float.Parse(blindTimerInput.Text));
                if (_blindTimer != null)
                {
                    Debug.WriteLine("Resume Timer " + DateTime.Now);
                    //resume the timer based on when we paused it
                    // timeLeft = length - elapsed 
                    TimeSpan timeLeft = _blindTimerLength - (_blindTimerPauseTime - _blindTimerStartTime);
                    Debug.WriteLine("Time left = " + timeLeft);
                    _blindTimer.Interval = timeLeft;
                    _blindTimer.Start();
                }
                else
                {
                    Debug.WriteLine("Start Timer " + DateTime.Now);

                    //make a new timer with full interval
                    _blindTimer = new DispatcherTimer();
                    _blindTimer.Interval = _blindTimerLength;
                    _blindTimer.Tick += onBlindTimerTick;
                    _blindTimer.Start();
                    _blindTimerStartTime = DateTime.Now;
                }
            }
            else
            {
                startBlindTimerButton.Content = "Start";

                Debug.WriteLine("Pause Timer " + DateTime.Now);

                //stop the timer and remember when we stopped it so we know how much time elapsed
                _blindTimerPauseTime = DateTime.Now;
                _blindTimer.Stop();
            }
        }

        private void resetBlindTimerButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Reset Timer " + DateTime.Now);
            _blindTimer.Stop();
            _blindTimer = null;
            startBlindTimerButton.Content = "Start";
        }


        private void onBlindTimerTick(object sender, object idk)
        {
            // make sure interval is set in case we changed it due to a pause
            _blindTimer.Interval = _blindTimerLength;
            _blindTimerStartTime = DateTime.Now;

            Debug.WriteLine("Timer Tick " + DateTime.Now);

            List<string> args = new List<string>();
            args.Add(((int)ARDUINO_MODE.PLAYER_COLORS).ToString());
            for (int i = 0; i < ViewModel.Players.Count; ++i)
            {
                args.Add(ColorToGammaShiftedHex(Colors.Red));
            }
            string urlArgs = string.Join("/", args);
            SetNextUpdateTask(urlArgs);

            // 2 seconds before reverting to normal colors
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(2);
            t.Tick += (object state, object wat) =>
            {
                t.Stop();
                DoUpdateArduino();
            };
            t.Start();
        }

        public override void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;

            MessageDialog dialog = new MessageDialog("Are you sure you want to quit the current Poker game?", "Warning");

            string navigationStr = Frame.GetNavigationState();

            UICommand quitBtn = new UICommand("Quit");
            quitBtn.Invoked = (s) => { base.HardwareButtons_BackPressed(sender, e); };
            dialog.Commands.Add(quitBtn);

            UICommand cancelBtn = new UICommand("Cancel");
            dialog.Commands.Add(cancelBtn);

            dialog.CancelCommandIndex = 1;

            dialog.ShowAsync();

            Debug.WriteLine("Back button pressed and handled");
        }
    }
}
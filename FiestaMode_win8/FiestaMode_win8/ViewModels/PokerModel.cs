using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Windows.UI;

namespace FiestaMode.ViewModels
{
    public class PokerModel : INotifyPropertyChanged
    {
        public PokerModel()
        {
            this.Players = new ObservableCollection<PokerPlayer>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<PokerPlayer> Players { get; set; }

        protected int _dealerActiveIndex = 0;
        public int DealerActiveIndex
        {
            get
            {
                return _dealerActiveIndex;
            }
            set
            {
                List<int> activeIndices = new List<int>();
                for (int i = 0; i < Players.Count; ++i)
                {
                    if (Players[i].IsInGame)
                        activeIndices.Add(i);
                }

                _dealerActiveIndex = (value + activeIndices.Count) % activeIndices.Count;
            }
        }

        public int DealerIndex
        {
            get
            {
                return Players.IndexOf(PokerPlayer.Dealer);
            }
        }

        public int SmallIndex
        {
            get
            {
                return Players.IndexOf(PokerPlayer.SmallBlind);
            }
        }

        public int BigIndex
        {
            get
            {
                return Players.IndexOf(PokerPlayer.BigBlind);
            }
        }

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
            this.Players.Add(new PokerPlayer( Colors.Yellow ));
            this.Players.Add(new PokerPlayer( Colors.Green ));
            this.Players.Add(new PokerPlayer( Colors.Blue ));
            this.Players.Add(new PokerPlayer( Colors.Purple ));
            this.Players.Add(new PokerPlayer( Colors.Magenta ));
            this.Players.Add(new PokerPlayer( Colors.Red ));
            this.Players.Add(new PokerPlayer( Colors.Orange ));
            this.Players.Add(new PokerPlayer( Colors.Gray));
            
            this.IsDataLoaded = true;
        }

        public void CycleDealer()
        {
            DealerActiveIndex += 1;

            List<int> activeIndices = new List<int>();
            for (int i = 0; i < Players.Count; ++i)
            {
                if (Players[i].IsInGame)
                    activeIndices.Add(i);
            }

            PokerPlayer.Dealer = Players[activeIndices[_dealerActiveIndex]];
            PokerPlayer.SmallBlind = Players[activeIndices[(_dealerActiveIndex + 1) % activeIndices.Count]];
            PokerPlayer.BigBlind = Players[activeIndices[(_dealerActiveIndex + 2) % activeIndices.Count]];
        }

        public void TogglePlayerInGame(PokerPlayer player)
        {
            player.IsInGame = !player.IsInGame;
            int index = Players.IndexOf(player);
            int delta = 0;
            if (player.IsInGame)
            {
                if (index <= DealerActiveIndex)
                {
                    delta = 1;
                }
                else
                {
                    delta = 0;
                }
            }
            else
            {
                if (index <= DealerActiveIndex)
                {
                    delta = -1;
                }
                else
                {
                    delta = 0;
                }
            }

            DealerActiveIndex += delta;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
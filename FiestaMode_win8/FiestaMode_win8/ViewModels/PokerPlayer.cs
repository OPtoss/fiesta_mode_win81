using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace FiestaMode.ViewModels
{
    public class PokerPlayer : ColorItem
    {


        protected static PokerPlayer _dealer;
        public static PokerPlayer Dealer
        {
            get
            {
                return _dealer;
            }
            set
            {
                PokerPlayer prev = _dealer;
                _dealer = value;
                if (prev != null)
                {
                    prev.PokerColor = prev.PokerColor;
                    prev.Text = prev.Text;
                }
                _dealer.PokerColor = _dealer.PokerColor;
                _dealer.Text = _dealer.Text;
            }
        }

        protected static PokerPlayer _smallBlind;
        public static PokerPlayer SmallBlind
        {
            get
            {
                return _smallBlind;
            }
            set
            {
                PokerPlayer prev = _smallBlind;
                _smallBlind = value;
                if (prev != null)
                {
                    prev.PokerColor = prev.PokerColor;
                    prev.Text = prev.Text;
                }
                _smallBlind.PokerColor = _smallBlind.PokerColor;
                _smallBlind.Text = _smallBlind.Text;
            }
        }

        protected static PokerPlayer _bigBlind;
        public static PokerPlayer BigBlind
        {
            get
            {
                return _bigBlind;
            }
            set
            {
                PokerPlayer prev = _bigBlind;
                _bigBlind = value;
                if (prev != null)
                {
                    prev.PokerColor = prev.PokerColor;
                    prev.Text = prev.Text;
                }
                _bigBlind.PokerColor = _bigBlind.PokerColor;
                _bigBlind.Text = _bigBlind.Text;
            }
        }

        protected bool _isInGame;
        public bool IsInGame {
            get
            {
                return _isInGame;
            }
            set
            {
                _isInGame = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("PokerColor");
                NotifyPropertyChanged("Text"); 
            }
        }

        public override Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("PokerColor"); // Color affects PokerColor
            }
        }

        public override string Text
        {
            get
            {
                if (this == Dealer)
                {
                    return "D";
                }
                else if (this == SmallBlind)
                {
                    return "S";
                }
                else if (this == BigBlind)
                {
                    return "B";
                }
                else
                {
                    return "";
                }
            }
            set
            {
                NotifyPropertyChanged();
            }
        }

        public Color PokerColor
        {
            get
            {
                if (!IsInGame)
                {
                    return Color.FromArgb(255,25,25,25);
                }
                else
                {
                    return _color;
                }
            }
            set
            {
                NotifyPropertyChanged();
            }
        }

        public PokerPlayer()
        {
            IsInGame = true;
            Color = Colors.Red;
            PokerColor = Colors.Red;
            Text = "";
        }

        public PokerPlayer(Color color)
        {
            IsInGame = true;
            Color = color;
            PokerColor = color;
            Text = "";
        }

        public PokerPlayer(String text, Color color, bool isInGame)
        {
            IsInGame = isInGame;
            Color = color;
            PokerColor = color;
            Text = text;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace FiestaMode.ViewModels
{
    public class ColorItem : INotifyPropertyChanged
    {
        protected string _text;
        public virtual string Text {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                NotifyPropertyChanged();
            }
        }

        protected Color _color;
        public virtual Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

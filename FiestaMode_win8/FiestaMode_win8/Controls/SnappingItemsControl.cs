using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FiestaMode.Controls
{
    public class SnappingItemsControl : ItemsControl, IScrollSnapPointsInfo
    {

        public bool AreHorizontalSnapPointsRegular
        {
            get { return (this.ItemsPanelRoot as StackPanel).AreHorizontalSnapPointsRegular; }
        }

        public bool AreVerticalSnapPointsRegular
        {
            get { return (this.ItemsPanelRoot as StackPanel).AreVerticalSnapPointsRegular; }
        }

        public IReadOnlyList<float> GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment)
        {
            StackPanel sp = this.ItemsPanelRoot as StackPanel;
            return sp.GetIrregularSnapPoints( orientation, alignment);
        }

        public float GetRegularSnapPoints(Orientation orientation, SnapPointsAlignment alignment, out float offset)
        {
            StackPanel sp = this.ItemsPanelRoot as StackPanel;
            return sp.GetRegularSnapPoints(orientation, alignment, out offset);
        }

        public event EventHandler<object> HorizontalSnapPointsChanged;

        public event EventHandler<object> VerticalSnapPointsChanged;
    }
}

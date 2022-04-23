using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace ERS.View
{
    public static class Animation
    {
        public static DoubleAnimation StartOpacAnim()
        {
            DoubleAnimation _oa = new DoubleAnimation();
            _oa.From = 0;
            _oa.To = 1;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(800d));
            return _oa;
        }
    }
}

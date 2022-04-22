using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERS.View.Picture
{
    /// <summary>
    /// Логика взаимодействия для PictureOnScreen.xaml
    /// </summary>
    public partial class PictureOnScreen : UserControl
    {
        private Point? _movePoint;
        public PictureOnScreen()
        {
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           Image.Height= this.ActualHeight;
            Image.Width= this.ActualWidth;

        }
        private void Btn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _movePoint = e.GetPosition(Image);
            Image.CaptureMouse();
        }

        private void Btn_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _movePoint = null;
            Image.ReleaseMouseCapture();
        }

        private void Btn_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_movePoint == null)
                return;
            var p = e.GetPosition(this) - (Vector)_movePoint.Value;
            
            Canvas.SetLeft(Image, p.X);
            Canvas.SetTop(Image, p.Y);
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Image.Height+=10;
                Image.Width+=10;
            }
                

            else if (e.Delta < 0)
            {
                if ((Image.Height-10)<0 ||   (Image.Width-10)<0)
                    return;
                Image.Height-=10;
                Image.Width-=10;
            }
        }
    }
}

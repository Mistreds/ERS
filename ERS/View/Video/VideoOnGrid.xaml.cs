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

namespace ERS.View.Video
{
    /// <summary>
    /// Логика взаимодействия для VideoOnGrid.xaml
    /// </summary>
    public partial class VideoOnGrid : UserControl
    {
        public VideoOnGrid()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Model.Video.DeleteVideo(DataContext as Model.Video);
            ViewModel.MainViewModel.videoViewModel.GetVideo();
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.MainViewModel.videoViewModel.SelectVideo(DataContext as Model.Video);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, Animation.StartOpacAnim());
        }
    }
}

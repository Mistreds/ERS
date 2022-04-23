using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для VideoMain.xaml
    /// </summary>
    public partial class VideoMain : UserControl
    {
        public VideoMain(ViewModel.VideoViewModel videoViewModel)
        {
            InitializeComponent();
            DataContext = videoViewModel;
         
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, Animation.StartOpacAnim());
        }
    }
}

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
    /// Логика взаимодействия для PictureOnGrid.xaml
    /// </summary>
    public partial class PictureOnGrid : UserControl
    {
        public PictureOnGrid()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Model.Picture.DeletePicture(DataContext as Model.Picture);
            ViewModel.MainViewModel.pictureViewModel.GetPicture();
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.MainViewModel.pictureViewModel.SelectPicture(DataContext as Model.Picture);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, Animation.StartOpacAnim());
        }
    }
}

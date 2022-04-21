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
    /// Логика взаимодействия для PictureMain.xaml
    /// </summary>
    public partial class PictureMain : UserControl
    {
        public PictureMain(ViewModel.PictureViewModel pictureViewModel)
        {
            InitializeComponent();
            this.DataContext = pictureViewModel;
        }
    }
}

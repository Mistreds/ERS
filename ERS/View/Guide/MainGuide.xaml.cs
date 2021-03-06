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

namespace ERS.View.Guide
{
    /// <summary>
    /// Логика взаимодействия для MainGuide.xaml
    /// </summary>
    public partial class MainGuide : UserControl
    {
        public MainGuide(ViewModel.GuideViewModel guideViewModel)
        {
            InitializeComponent();
            DataContext = guideViewModel;
        }

        private void MainControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, Animation.StartOpacAnim());
        }
    }
}

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

namespace ERS.View.Document
{
    /// <summary>
    /// Логика взаимодействия для MainDocument.xaml
    /// </summary>
    public partial class MainDocument : UserControl
    {
        public MainDocument(ViewModel.DocumentViewModel documentViewModel)
        {
            InitializeComponent();
            this.DataContext = documentViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, Animation.StartOpacAnim());
        }
    }
}

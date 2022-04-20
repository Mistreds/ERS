
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERS.View.Guide
{
    /// <summary>
    /// Логика взаимодействия для WebPage.xaml
    /// </summary>
    public partial class WebPage : UserControl
    {
        public WebPage(string url)
        {
            InitializeComponent();
            //Browser.Navigated += (sender, args) => { HideScriptErrors((WebBrowser)sender, true); };
            WindowsFormsHost host = new WindowsFormsHost();
            //GeckoWebBrowser browser = new GeckoWebBrowser();
            //host.Child = browser;
            //MainGrid.Children.Add(host);
            //browser.Navigate("http://www.google.com");
            //if (url!=null) ;
             //   Browser.Navigate(new Uri(url));
             dfs.Address = url;
            dfs.AddressChanged += Dfs_AddressChanged;
        }

        private void Dfs_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        
    }
}

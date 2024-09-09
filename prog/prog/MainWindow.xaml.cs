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

namespace prog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WelcomePage page = new WelcomePage();
            MainFrame.Content = page;
        }

        private void GoOnTask1Btn_Click(object sender, RoutedEventArgs e)
        {
            Page page = new Task1();
            MainFrame.Navigate(page);
        }

        private void GoOnTask2Btn_Click(object sender, RoutedEventArgs e)
        {
            Page page = new Task2();
            MainFrame.Navigate(page);
        }
    }
}

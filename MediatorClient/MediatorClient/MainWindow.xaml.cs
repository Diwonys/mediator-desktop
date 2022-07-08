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

namespace MediatorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void ExpandCollapse(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                ExpandImage.Source = new BitmapImage(new Uri("./UI/Images/Icons/ExpandCollapse/eva_collapse-fill.png", UriKind.Relative));
                WindowState = WindowState.Maximized;
            }
            else
            {
                ExpandImage.Source = new BitmapImage(new Uri("./UI/Images/Icons/ExpandCollapse/uil_expand-alt.png", UriKind.Relative));
                WindowState = WindowState.Normal;
            }
        }

        private void RollDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnTopPanelMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

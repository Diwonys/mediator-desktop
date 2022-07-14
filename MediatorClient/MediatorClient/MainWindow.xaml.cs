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
        public event Action DrugMoved;
        private readonly ResourceDictionary _iconsResourceDictionary;
        public MainWindow()
        {
            InitializeComponent();
            _iconsResourceDictionary = new ResourceDictionary();
            _iconsResourceDictionary.Source =
                new Uri("./UI/Styles/Icons/ControlIcons.xaml", UriKind.Relative);

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            DrugMoved += OnMainWindowDrugMoved;
        }

        private void OnCloseClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void ExpandCollapse(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                ExpandImage.Style = (Style)_iconsResourceDictionary["CollapseControlIcon"];
                WindowState = WindowState.Maximized;
            }
            else
            {
                ExpandImage.Style = (Style)_iconsResourceDictionary["ExpandControlIcon"];
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
            {
                DragMove();
                //DrugMoved?.Invoke();
            }
        }

        private void OnMainWindowDrugMoved()
        {
            //if (WindowState == WindowState.Maximized)
            //{
            //    WindowState = WindowState.Normal;
            //}
        }
    }
}

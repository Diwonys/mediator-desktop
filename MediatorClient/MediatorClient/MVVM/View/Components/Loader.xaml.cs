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

namespace MediatorClient.MVVM.View.Components
{
    /// <summary>
    /// Логика взаимодействия для Loader.xaml
    /// </summary>
    public partial class Loader : UserControl
    {
        public Loader()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        Dispatcher.Invoke(() =>
            //        {
            //            LoaderImage.RenderTransform = new RotateTransform(45);
            //        });
            //    }
            //}).ConfigureAwait(false);
        }
    }
}

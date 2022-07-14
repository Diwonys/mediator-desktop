using MediatorClient.Core.Helpers;
using MediatorClient.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MediatorClient.MVVM.View.Update
{
    /// <summary>
    /// Логика взаимодействия для UpdateView.xaml
    /// </summary>
    public partial class UpdateView : UserControl
    {
        public UpdateView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                RedirectToMain();
            });
        }

        public void RedirectToMain()
        {
            Dispatcher.Invoke(() =>
            {
                var obj = VisualTreeHelper.GetParent(this) as ContentPresenter;
                var value = obj.TemplatedParent.GetPropertyValue("DataContext") as TemplateViewModel;
                value.CurrentView = value.MainTemplate;
            });
        }
    }
}

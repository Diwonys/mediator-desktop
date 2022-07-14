using MediatorClient.Core.Helpers;
using MediatorClient.MVVM.ViewModel;
using MediatorClient.MVVM.ViewModel.Preload;
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

namespace MediatorClient.MVVM.View.Preload
{
    /// <summary>
    /// Логика взаимодействия для PreloadView.xaml
    /// </summary>
    public partial class PreloadView : UserControl
    {
        public PreloadView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if(false) //logined check
                RedirectToUpdate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (true) //logined check
                RedirectToUpdate();
        }

        public void RedirectToUpdate()
        {
            var obj = VisualTreeHelper.GetParent(this) as ContentPresenter;
            var value = obj.TemplatedParent.GetPropertyValue("DataContext") as TemplateViewModel;
            value.CurrentView = value.UpdateTemplate;
        }
    }

}

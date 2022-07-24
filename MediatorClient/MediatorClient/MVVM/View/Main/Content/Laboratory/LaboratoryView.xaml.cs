using MediatorClient.Services.Driver;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediatorClient.MVVM.View.Main.Content.Laboratory
{
    public partial class LaboratoryView : UserControl
    {
        private AsioWrapper _asio;
        public LaboratoryView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            popup1.IsOpen = true;
        }

        private void OnASIOStarted(object sender, System.Windows.RoutedEventArgs e)
        {
            _asio = new AsioWrapper(Dispatcher);
        }

        private void Play(object sender, System.Windows.RoutedEventArgs e)
        {
            _asio.Play();
        }
    }
}

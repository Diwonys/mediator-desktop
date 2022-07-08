using MediatorClient.MVVM.Model;
using MediatorClient.Services;
using NAudio.Wave;
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

namespace MediatorClient.MVVM.View.Main.Content.Settings
{
    /// <summary>
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly AsioSettings _settings;
        private readonly string _settingsKey = "AsioSettings";
        public SettingsView()
        {
            InitializeComponent();

            foreach (var driverName in AsioOut.GetDriverNames())
            {
                AsioDrivers.Items.Add(driverName);
            }
            AsioDrivers.Items.Add("1");
            AsioDrivers.Items.Add("2");
            AsioDrivers.Items.Add("3");

            _settings = LocalStorageService.Get<AsioSettings>(_settingsKey);
            AsioDrivers.Text = _settings.DriverName;
        }

        private async void OnAsioDriversSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var settings = LocalStorageService.Get<AsioSettings>(_settingsKey);
            settings.DriverName = AsioDrivers.SelectedItem.ToString();
            await LocalStorageService.AddOrReplaceAsync(_settingsKey, settings);
        }
    }
}

using MediatorClient.MVVM.Model;
using MediatorClient.Services;
using NAudio.Wave;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        private readonly NotificationManager _notificationManager = new NotificationManager();
        private AsioSettings _asioSettings;
        private NetworkSettings _networkSettings;
        public SettingsView()
        {
            InitializeComponent();
            InitAsioSettings();
            InitNetworkSettings();

            _asioSettings = LocalStorageService.Get<AsioSettings>();
            _networkSettings = LocalStorageService.Get<NetworkSettings>();
            AsioDrivers.Text = _asioSettings.DriverName;
            IPConnection.Text = _networkSettings.IPConnection;
        }

        private void InitNetworkSettings()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var addresses = host.AddressList
               .Where(i => i.AddressFamily == AddressFamily.InterNetwork);

            foreach (var address in addresses)
            {
                IPConnection.Items.Add(address.ToString());
            }
        }

        private void InitAsioSettings()
        {
            foreach (var driverName in AsioOut.GetDriverNames())
            {
                AsioDrivers.Items.Add(driverName);
            }
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            _asioSettings = LocalStorageService.Get<AsioSettings>();
            _asioSettings.DriverName = AsioDrivers.SelectedItem.ToString();
            await LocalStorageService.AddOrReplaceAsync<AsioSettings>(_asioSettings);

            _networkSettings = LocalStorageService.Get<NetworkSettings>();
            _networkSettings.IPConnection = IPConnection.SelectedItem.ToString();
            await LocalStorageService.AddOrReplaceAsync<NetworkSettings>(_networkSettings);


            _notificationManager.Show(new NotificationContent
            {
                Title = string.Empty,
                Message = "Settings updated",
                Type = NotificationType.Information
            }, areaName: "WindowArea");

        }
    }
}

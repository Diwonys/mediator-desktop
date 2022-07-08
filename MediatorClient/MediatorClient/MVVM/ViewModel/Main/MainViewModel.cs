using MediatorClient.Core;
using MediatorClient.MVVM.View.Main.Content.Laboratory;
using MediatorClient.MVVM.ViewModel.Main.Content.Home;
using MediatorClient.MVVM.ViewModel.Main.Content.Laboratory;
using MediatorClient.MVVM.ViewModel.Main.Content.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.MVVM.ViewModel.Main
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand SettingsCommand { get; set; }
        public RelayCommand LaboratoryCommand { get; set; }

        public HomeViewModel Home { get; set; }
        public SettingsViewModel Settings { get; set; }
        public LaboratoryViewModel Laboratory { get; set; }

        public MainViewModel()
        {
            Home = new HomeViewModel();
            Settings = new SettingsViewModel();
            Laboratory = new LaboratoryViewModel();

            CurrentView = Home;

            HomeCommand = new RelayCommand(o =>
            {
                CurrentView = Home;
            });

            SettingsCommand = new RelayCommand(o =>
            {
                CurrentView = Settings;
            });

            LaboratoryCommand = new RelayCommand(o =>
            {
                CurrentView = Laboratory;
            });
        }
    }
}

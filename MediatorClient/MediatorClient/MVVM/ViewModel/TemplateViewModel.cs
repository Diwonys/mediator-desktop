using MediatorClient.Core;
using MediatorClient.MVVM.ViewModel.Preload;
using MediatorClient.MVVM.ViewModel.Main;
using MediatorClient.MVVM.ViewModel.Update;

namespace MediatorClient.MVVM.ViewModel
{
    class TemplateViewModel : ObservableObject
    {
        public RelayCommand MainTemplateCommand { get; set; }
        public RelayCommand PreloadTemplateCommand { get; set; }
        public RelayCommand UpdateTemplateCommand { get; set; }

        public MainViewModel MainTemplate { get; set; }
        public PreloadViewModel PreloadTemplate { get; set; }
        public UpdateViewModel UpdateTemplate { get; set; }

        public TemplateViewModel()
        {
            MainTemplate = new MainViewModel();
            PreloadTemplate = new PreloadViewModel();
            UpdateTemplate = new UpdateViewModel();

            CurrentView = MainTemplate;
            
            MainTemplateCommand = new RelayCommand(o =>
            {
                CurrentView = MainTemplate;
            });

            PreloadTemplateCommand = new RelayCommand(o =>
            {
                CurrentView = PreloadTemplate;
            });

            UpdateTemplateCommand = new RelayCommand(o =>
            {
                CurrentView = UpdateTemplate;
            });
        }
    }
}
using MediatorClient.Core;
using MediatorClient.MVVM.ViewModel.Preload;
using MediatorClient.MVVM.ViewModel.Main;

namespace MediatorClient.MVVM.ViewModel
{
    class TemplateViewModel : ObservableObject
    {
        public RelayCommand MainTemplateCommand { get; set; }
        public RelayCommand PreloadTemplateCommand { get; set; }

        public MainViewModel MainTemplate { get; set; }
        public PreloadViewModel PreloadTemplate { get; set; }


        public TemplateViewModel()
        {
            MainTemplate = new MainViewModel();
            PreloadTemplate = new PreloadViewModel();
            CurrentView = PreloadTemplate;
            
            MainTemplateCommand = new RelayCommand(o =>
            {
                CurrentView = MainTemplate;
            });

            PreloadTemplateCommand = new RelayCommand(o =>
            {
                CurrentView = PreloadTemplate;
            });
        }
    }
}

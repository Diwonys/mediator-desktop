using MediatorClient.MVVM.View.Components;
using MediatorClient.Services.Driver;
using MediatorClient.Services.Driver.Tuner;
using MediatorClient.Services.Driver.Utils;
using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MediatorClient.MVVM.View.Main.Content.Laboratory
{
    public partial class LaboratoryView : UserControl
    {
        private AsioWrapper _asio;
        private TunerService _tuner;
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

            _tuner = new TunerService(_asio);
            _tuner.FrameChanged += OnTunerFrameChanged;
            _tuner.Run();
            
            FrequencyVisualizerComponent.Chart.Plot.AddSignal(_tuner.FftValues, _tuner.FFtPeriod);
            FrequencyVisualizerComponent.Chart.Plot.YLabel("Spectral Power");
            FrequencyVisualizerComponent.Chart.Plot.XLabel("Frequency (kHz)");

            FrequencyVisualizerComponent.Chart.Refresh();

            _asio.Play();
        }

        private void OnTunerFrameChanged(FrameChangedEventArgs args)
        {
            TunerComponent.LeftNote.Text = args.LeftNote.Name + args.LeftNote.Frequency.ToString() ;
            TunerComponent.RightNote.Text = args.RightNote.Name + args.RightNote.Frequency.ToString();
            TunerComponent.MiddleNote.Text = args.MiddleNote.Name + args.PeakFrequency.ToString() + "|"  + args.MiddleNote.Frequency.ToString();


            double vecotrPart = Math.Abs(args.MiddleNote.Frequency - args.PeakFrequency);
            double min = args.MiddleNote.Frequency - vecotrPart - 10;
            double max = args.MiddleNote.Frequency + vecotrPart + 10;
            if (Math.Abs(min + max) % 2 != 0)
                max++;
            
            TunerComponent.PercentBar.Minimum = min;
            TunerComponent.PercentBar.Maximum = max;
            TunerComponent.PercentBar.Value = args.PeakFrequency;
            

            double plotYMax = FrequencyVisualizerComponent.Chart.Plot.GetAxisLimits().YMax;
            FrequencyVisualizerComponent.Chart.Plot.SetAxisLimits(
            xMin: 0,
            xMax: 1.5,
            yMin: 60,
            yMax: Math.Max(args.FFtPeakMagnitude, plotYMax));

            FrequencyVisualizerComponent.Chart.RefreshRequest();
        }

        private void Play(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_asio == null)
                _asio = new AsioWrapper(Dispatcher);
            _asio.Play();
        }
    }
}

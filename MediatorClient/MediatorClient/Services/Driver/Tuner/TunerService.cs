using MediatorClient.MVVM.View.Components;
using MediatorClient.Services.Driver.Tuner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MediatorClient.Services.Driver.Utils
{
    public class TunerService
    {
        private readonly AsioWrapper _asio;
        public event Action<FrameChangedEventArgs> FrameChanged;

        private DispatcherTimer _timer;
        private Note _leftNote = new Note();
        private Note _rightNote = new Note();
        private Note _middleNote = new Note();
        private double _peakFrequency;
        public double[] FftValues { get; private set; }
        public double FFtPeriod { get; private set; }
        public Dictionary<string, double[]> Notes { get; set; } = new Dictionary<string, double[]>()
        {
            { "C", new double[] {   16.35,  32.7,   65.41,  130.82, 261.63 } },
            { "C#", new double[] {  17.32,  34.65,  69.3,   138.59, 277.18 } },
            { "D", new double[] {   18.35,  36.95,  73.91,  147.83, 293.66 } },
            { "D#", new double[] {  19.44,  38.88,  77.78,  155.56, 311.13 } },
            { "E", new double[] {   20.61,  41.21,  82.41,  164.81, 329.63 } },
            { "F", new double[] {   21.82,  43.65,  87.31,  174.62, 349.23 } },
            { "F#", new double[] {  23.12,  46.25,  92.5,   185,    369.99 } },
            { "G", new double[] {   24.5,   49,     98,     196,    392 } },
            { "G#", new double[] {  25.95,  51.9,   103.8,  207,    415.3 } },
            { "A", new double[] {   27.5,   55,     110,    220,    440 } },
            { "A#", new double[] {  29.13,  58.26,  116.54, 233.08, 466.16 } },
            { "B", new double[] {   30.87,  61.74,  123.48, 246.96, 493.88 } },
        };

        public double[] NotesFrequencies;

        public TunerService(AsioWrapper asio, int updateMilliseconds = 20)
        {
            _asio = asio;

            InitComputingArrays();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(updateMilliseconds);
            _timer.Tick += OnTunerUpdate;
        }

        private void InitComputingArrays()
        {
            int length = Notes.First().Value.Length * Notes.Count();
            NotesFrequencies = new double[length];

            int counter = 0;
            foreach (var note in Notes)
            {
                foreach (var value in note.Value)
                {
                    NotesFrequencies[counter] = value;
                    counter++;
                }
            }

            Array.Sort(NotesFrequencies);
        }
        private void OnTunerUpdate(object? sender, EventArgs e)
        {
            double[] paddedAudio = FftSharp.Pad.ZeroPad(_asio.InterleavedSamples);
            double[] fftMagnitude = FftSharp.Transform.FFTpower(paddedAudio);
            Array.Copy(fftMagnitude, FftValues, fftMagnitude.Length);

            int peakIndex = 0;
            for (int i = 0; i < fftMagnitude.Length; i++)
            {
                if (fftMagnitude[i] > fftMagnitude[peakIndex])
                    peakIndex = i;
            }

            double fftPeriod = FftSharp.Transform.FFTfreqPeriod(_asio.SampleRate, fftMagnitude.Length);
            float peakFrequency = (float)Math.Round(fftPeriod * peakIndex, 2);

            double fftPeakMagnitude = fftMagnitude.Max();

            if (fftPeakMagnitude > 63 && peakFrequency > 15 && peakFrequency < 500)
            { 
                _middleNote = GetNearest(peakFrequency);
                _leftNote = GetPrevious(_middleNote.Frequency);
                _rightNote = GetNext(_middleNote.Frequency);
                _peakFrequency = peakFrequency;
            }

            FrameChanged.Invoke(new FrameChangedEventArgs
            {
                FFtPeakMagnitude = fftPeakMagnitude,
                PeakFrequency = _peakFrequency,
                LeftNote = _leftNote,
                RightNote = _rightNote,
                MiddleNote = _middleNote,
            });
        }

        private Note GetNearest(double frequency)
        {
            var nearest = NotesFrequencies.MinBy(x => Math.Abs(x - frequency));

            foreach (var note in Notes)
            {
                foreach (var value in note.Value)
                {
                    if (value == nearest)
                    {
                        return new Note
                        {
                            Name = note.Key,
                            Frequency = value
                        };
                    }
                }

            }

            return null;
        }

        private Note GetNext(double frequency)
        {
            var nextHigher = NotesFrequencies[0];
            for (int i = 0; i < NotesFrequencies.Length; i++)
            {
                if (NotesFrequencies[i] > frequency)
                {
                    nextHigher = NotesFrequencies[i];
                    break;
                }
            }
            
            foreach (var note in Notes)
            {
                foreach (var value in note.Value)
                {
                    if (value == nextHigher)
                    {
                        return new Note
                        {
                            Name = note.Key,
                            Frequency = value
                        };
                    }
                }

            }

            return null;
        }

        private Note GetPrevious(double frequency)
        {
            var previousLower = NotesFrequencies[NotesFrequencies.Length - 1];
            for (int i = NotesFrequencies.Length - 1; i >= 0; i--)
            {
                if (NotesFrequencies[i] < frequency)
                {
                    previousLower = NotesFrequencies[i];
                    break;
                }
            }

            foreach (var note in Notes)
            {
                foreach (var value in note.Value)
                {
                    if (value == previousLower)
                    {
                        return new Note
                        {
                            Name = note.Key,
                            Frequency = value
                        };
                    }
                }

            }

            return null;
        }
        
        public void Run()
        {
            _asio.AddTuner();

            double[] paddedAudio = FftSharp.Pad.ZeroPad(_asio.InterleavedSamples);
            double[] fftMag = FftSharp.Transform.FFTmagnitude(paddedAudio);
            FftValues = new double[fftMag.Length];
            FFtPeriod = FftSharp.Transform.FFTfreqPeriod(_asio.SampleRate, fftMag.Length);

            _timer.Start();
        }

        public void Stop()
        {
            _asio.RemoveTuner();
            _timer.Stop();
        }
    }
}

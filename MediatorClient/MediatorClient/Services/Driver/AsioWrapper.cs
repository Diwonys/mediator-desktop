using MediatorClient.MVVM.Model;
using MediatorClient.MVVM.View.Components;
using MediatorClient.Services.Driver.Utils;
using NAudio.Wave;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MediatorClient.Services.Driver
{
    public class AsioWrapper
    {
        private AsioOut _asioOut;
        private AsioSettings _asioSettings;
        private BufferedWaveProvider _bufferedWaveProvider;
        private PlaybackState _playbackState;
        public int SampleRate { get; set; } = 44_100;
        private Dispatcher _dispatcher;

        public double[] InterleavedSamples;
        
        
        public AsioWrapper(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _asioSettings = LocalStorageService.Get<AsioSettings>();
            _asioOut = InitializeInstance();
            DefaultConfiguration();

        }

        private AsioOut InitializeInstance()
        {
            //var asioNames = AsioOut.GetDriverNames();
            //if (asioNames.Length < 1)
            //    throw new InvalidOperationException("ASIO4ALL not found");
            //if (!AsioOut.isSupported())
            //    throw new InvalidOperationException("ASIO4ALL not supported");

            return new AsioOut(_asioSettings.DriverName);
        }
        private void DefaultConfiguration()
        {
            var channelCount = _asioOut.DriverInputChannelCount;
            var waveFormat = new WaveFormat(SampleRate, 16, 1);
            _bufferedWaveProvider = new BufferedWaveProvider(waveFormat)
            {
                ReadFully = true,
                BufferLength = 2048,
                DiscardOnBufferOverflow = true
            };

            _asioOut.AudioAvailable += OnAsioOutAudioAvailable;
            _asioOut.DriverResetRequest += OnAsioOutDriverResetRequest;

            //var multiplexer = new MultiplexingWaveProvider(new IWaveProvider[] { _bufferedOutWaveProvider }); //fix this later (non critical)
            //multiplexer.ConnectInputToOutput(1, 0);
            //multiplexer.ConnectInputToOutput(1, 1);

            _asioOut.InitRecordAndPlayback(_bufferedWaveProvider, 2, SampleRate);
        }

        private void OnAsioOutAudioAvailableInfo(object? sender, AsioAudioAvailableEventArgs e)
        {
            int fullBufferSize = e.SamplesPerBuffer * 4;
            int sampleBufferSize = e.SamplesPerBuffer;

            byte[] buffer = new byte[fullBufferSize];
            //for (int i = 0; i < e.InputBuffers.Length; i++)
            //{
                CopyBytes(e.InputBuffers[0], buffer, 0, fullBufferSize);
            //}

            for (int i = 0; i < buffer.Length / 2; i++)
                InterleavedSamples[i] = BitConverter.ToInt16(buffer, i * 2);

            //InterleavedSamples = 
            //for (int i = 0; i < buffer.Length / 2; i++)
            //    InterleavedSamples[i] = BitConverter.ToInt16(buffer, i * 2);

            //for (int i = 0, j = 0; i < fullBufferSize; i += 4, j++)
            //{
            //    AudioValues[j] = Convert.ToSingle(buffer[i] + buffer[i + 1] + buffer[i + 2] + buffer[i + 3]);
            //}

            //float[] samples = e.GetAsInterleavedSamples();
            //Array.Clear(InterleavedSamples);
            //Array.Copy(samples, InterleavedSamples, samples.Length);

        }

        private void OnAsioOutAudioAvailable(object? sender, AsioAudioAvailableEventArgs e)
        {
            int fullBufferSize = e.SamplesPerBuffer * 4;
            
            //for (int i = 0; i < e.InputBuffers.Length; i++)
            //{
                CopyBytes(e.InputBuffers[0], e.OutputBuffers[0], 0, fullBufferSize);                
            //}

            e.WrittenToOutputBuffers = true;
        }

        private unsafe void CopyBytes(byte[] source, IntPtr destination, int startIndex, int size)
        {
            byte* pointer = (byte*)destination.ToPointer();
            int counter = 0;
            for (int i = startIndex; i < size; i++)
            {
                *(pointer + i) = source[counter];
                counter++;
            }
        }
        private unsafe void CopyBytes(IntPtr source, IntPtr destination, int startIndex, int size)
        {
            byte* pointerSource = (byte*)source.ToPointer();
            byte* pointerDestination = (byte*)destination.ToPointer();

            for (int i = startIndex; i < size; i++)
            {
                *(pointerDestination + i) = *(pointerSource + i);
            }
        }
        private unsafe void CopyBytes(IntPtr source, byte[] destination, int startIndex, int size)
        {
            byte* pointer = (byte*)source.ToPointer();
            int counter = 0;
            for (int i = startIndex; i < size; i++)
            {
                destination[counter] = *(pointer + i);
                counter++;
            }
        }

        public void Play()
        {
            _playbackState = PlaybackState.Playing;
            _asioOut.Play();
        }

        private void OnAsioOutDriverResetRequest(object? sender, EventArgs e)
        {
            _dispatcher.BeginInvoke(() =>
            {
                _asioOut.Dispose();

                _asioOut = InitializeInstance();
                DefaultConfiguration();

                if (_playbackState == PlaybackState.Playing)
                {
                    _asioOut.Play();
                }
            });
        }

        public void AddTuner()
        {
            InterleavedSamples = new double[8192];
            _asioOut.AudioAvailable += OnAsioOutAudioAvailableInfo;
        }
        public void RemoveTuner()
        {
            Array.Clear(InterleavedSamples);
            _asioOut.AudioAvailable -= OnAsioOutAudioAvailableInfo;
        }
    }
}

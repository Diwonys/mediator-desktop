using MediatorClient.MVVM.Model;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.Services.Driver
{
    public class AsioWrapper
    {
        private AsioOut _asioOut;
        private AsioSettings _asioSettings;
        private BufferedWaveProvider _bufferedOutWaveProvider;
        private BufferedWaveProvider _bufferedInputWaveProvider;
        public int SampleRate { get; set; } = 44_100;

        
        public AsioWrapper()
        {
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
            var waveFormat = new WaveFormat(SampleRate, 2);
            _bufferedOutWaveProvider = new BufferedWaveProvider(waveFormat)
            {
                ReadFully = true,
                BufferLength = 2048,
                DiscardOnBufferOverflow = true
            };


            _asioOut.DriverResetRequest += OnAsioOutDriverResetRequest;

            //var multiplexer = new MultiplexingWaveProvider(new IWaveProvider[] { _bufferedOutWaveProvider }); //fix this later (non critical)
            //multiplexer.ConnectInputToOutput(1, 0);
            //multiplexer.ConnectInputToOutput(1, 1);

            _asioOut.InitRecordAndPlayback(_bufferedInputWaveProvider, 2, SampleRate);

            //Starttest();
        }

        private void OnAsioOutDriverResetRequest(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

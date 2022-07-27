using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.Services.Driver.Tuner
{
    public class FrameChangedEventArgs : EventArgs
    {
        public Note LeftNote { get; set; }
        public Note RightNote { get; set; }
        public Note MiddleNote { get; set; }
        public double PeakFrequency { get; set; }
        public double FFtPeakMagnitude { get; set; }
        public double PeakFrequencyPercentage { get; set; }
    }
}

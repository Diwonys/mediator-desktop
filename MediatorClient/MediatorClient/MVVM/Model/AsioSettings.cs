using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.MVVM.Model
{
    public class AsioSettings
    {
        public string DriverName { get; set; } = AsioOut.GetDriverNames().FirstOrDefault();
    }
}

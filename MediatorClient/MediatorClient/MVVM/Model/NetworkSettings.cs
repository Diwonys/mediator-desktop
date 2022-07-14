using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.MVVM.Model
{
    public class NetworkSettings
    {
        public string IPConnection { get; set; } = Dns.GetHostEntry(Dns.GetHostName()).AddressList
               .Where(i => i.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault().ToString();
    }
}

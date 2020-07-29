using System.Collections.Generic;
using System.Threading.Tasks;
using PacketDotNet;
using PortScannerDetectorInstrument.Entities;

namespace PortScannerDetectorInstrument.Strategies.TcpConnect
{
    public class PacketSource : AbstractPacketSource
    {
        public string Ip {get;set;}

        public int SynCount  {get;set;}

        public int RstAckCount {get;set;}
    }
}
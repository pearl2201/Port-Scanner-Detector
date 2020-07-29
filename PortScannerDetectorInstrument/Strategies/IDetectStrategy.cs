using System.Collections.Generic;
using System.Threading.Tasks;
using PacketDotNet;
using PortScannerDetectorInstrument.Entities;

namespace PortScannerDetectorInstrument.Strategies
{
    public interface IDetectStrategy
    {
        Task<List<SuspiciousSource>> Scan(List<TimeTcpPacket> tcpPackets);
        
    }
}
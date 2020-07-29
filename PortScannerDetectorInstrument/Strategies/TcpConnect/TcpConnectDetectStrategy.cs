using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketDotNet;
using PortScannerDetectorInstrument.Entities;

namespace PortScannerDetectorInstrument.Strategies.TcpConnect
{
    public class TcpConnectDetectStrategy : IDetectStrategy
    {
        public const ushort RSTACK = 0x014;
        public const ushort SYN = 0x002;

        public const int ACKRATIO = 10;

        public TcpConnectDetectStrategy()
        {
        }

        public async Task<List<SuspiciousSource>> Scan(List<TimeTcpPacket> tcpPackets)
        {
            Console.WriteLine("[*] Scan Tcp Packet");
            Dictionary<string, PacketSource> sources = new Dictionary<string, PacketSource>();
            List<SuspiciousSource> ret = new List<SuspiciousSource>();
            foreach (var packet in tcpPackets)
            {

                var ipPacket = (PacketDotNet.IPPacket)packet.TcpPacket.ParentPacket;

                var sourceAddress = ipPacket.SourceAddress.ToString();
                if (!sources.ContainsKey(sourceAddress))
                {
                    sources.Add(sourceAddress, new PacketSource() { Ip = sourceAddress});
                }

                if (packet.TcpPacket.Flags == SYN)
                {
                    sources[sourceAddress].SynCount += 1;
                }
                else if (packet.TcpPacket.Flags == RSTACK)
                {
                    sources[sourceAddress].RstAckCount += 1;
                }
            }
            return await Task.FromResult(ExtractSuspect(sources));

        }
        public List<SuspiciousSource> ExtractSuspect(Dictionary<string, PacketSource> sources)
        {
            Console.WriteLine("[*] ExtractSuspect");
            List<SuspiciousSource> ret = new List<SuspiciousSource>();
            foreach (var pair in sources)
            {
                //Console.WriteLine($"{pair.Key} sent {pair.Value.SynCount} SYNs and {pair.Value.RstAckCount} RST/ACKs");
                if (pair.Value.RstAckCount > 0 && pair.Value.SynCount > ACKRATIO * pair.Value.RstAckCount)
                {
                    ret.Add(new SuspiciousSource()
                    {
                        Ip = pair.Key,
                        Metadata = $"Sent {pair.Value.SynCount} SYNs and {pair.Value.RstAckCount} RST/ACKs",
                        Time = DateTime.Now
                    });
                }
            }
            return ret;
        }

    }
}


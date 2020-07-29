using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using PacketDotNet;

namespace PortScannerDetectorInstrument.Entities{
    public class TimeTcpPacket{
        public TcpPacket TcpPacket {get;set;}
        public DateTime Time {get;set;}
    }

}
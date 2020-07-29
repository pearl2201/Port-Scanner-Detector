using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace PortScannerDetectorServer.Entities{
    public class TcpPacket{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}
        [Required]
        public string SrcIp {get;set;}
        [Required]
        public string DstIp {get;set;}
        [Required]
        public int SrcPort {get;set;}
        [Required]
        public int DstPort {get;set;}
        [Required]
        public DateTime Time {get;set;}

    }
}
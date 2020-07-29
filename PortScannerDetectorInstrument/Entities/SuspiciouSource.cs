using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace PortScannerDetectorInstrument.Entities{
    public class SuspiciousSource{
        [Required]
        public string Ip {get;set;}
        [Required]
        public DateTime Time {get;set;}

        public string Metadata {get;set;}
    }
}
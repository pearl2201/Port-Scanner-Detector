using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace PortScannerDetectorServer.Entities{
    public class Address{
        [Key]
        public int Id {get;set;}

        public string Ip {get;set;}

        public bool Blocked {get;set;}
        public List<SuspiciousSource> SuspiciousSources {get;set;}
    }
}
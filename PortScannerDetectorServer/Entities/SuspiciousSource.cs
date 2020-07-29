using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace PortScannerDetectorServer.Entities
{
    public class SuspiciousSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Ip { get; set; }
        [Required]
        public DateTime Time { get; set; }

        public string Metadata { get; set; }

        public Address Address { get; set; }
    }
}
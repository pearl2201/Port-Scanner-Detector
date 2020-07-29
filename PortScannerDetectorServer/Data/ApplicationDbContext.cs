using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PortScannerDetectorServer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PortScannerDetectorServer.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<SuspiciousSource> SuspiciousSources {get;set;}

        public DbSet<Address> Addresses {get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
           
        }
    }
}
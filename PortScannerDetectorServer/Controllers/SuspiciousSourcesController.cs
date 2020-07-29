using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortScannerDetectorServer.Data;
using PortScannerDetectorServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortScannerDetectorServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuspiciousSourcesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public SuspiciousSourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuspiciousSource>>> Index()
        {
            return await _context.SuspiciousSources.ToListAsync();
        }

        [HttpGet("id:int")]
        public async Task<ActionResult<SuspiciousSource>> Detail(int id)
        {
            var packet = await _context.SuspiciousSources.FindAsync(id);
            if (packet == null)
            {
                return NotFound();
            }

            return packet;
        }

        [HttpPost]
        public async Task<ActionResult<TcpPacket>> PostPacket(List<SuspiciousSource> packets)
        {
            foreach (var packet in packets)
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Ip == packet.Ip);

                if (address == null)
                {
                    address = new Address()
                    {
                        Ip = packet.Ip,
                        Blocked = false
                    };
                    _context.Addresses.Add(address);
                }
                packet.Address = address;
            }
            _context.SuspiciousSources.AddRange(packets);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
using System;
using SharpPcap;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;

namespace PortScannerDetectorInstrument
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var textSetting = File.ReadAllText("appsettings.json");
            var appSettings = JsonSerializer.Deserialize<AppSettings>(textSetting);

            PortScannerDetector detector = new PortScannerDetector(appSettings);

            await detector.Run();
        }


    }

}

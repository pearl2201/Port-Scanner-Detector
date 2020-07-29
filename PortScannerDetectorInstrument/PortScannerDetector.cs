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
using System.Threading;
using PacketDotNet;
using PortScannerDetectorInstrument.Reporters;
using PortScannerDetectorInstrument.Strategies;
using PortScannerDetectorInstrument.Strategies.TcpConnect;
using PortScannerDetectorInstrument.Entities;

public class PortScannerDetector
{
    public List<TimeTcpPacket> packets;
    private List<IReporter> reporters;

    private IDetectStrategy detectStrategy;

    private AppSettings settings;
    public PortScannerDetector(AppSettings settings)
    {
        packets = new List<TimeTcpPacket>();
        reporters = new List<IReporter>();
        this.settings = settings;
        foreach (var reportOption in settings.ReportOptions)
        {
            Console.WriteLine("[*] ReportOption: " + reportOption);
            if (reportOption == "REST")
            {
                Console.WriteLine("[*] Add Rest Report Option");
                reporters.Add(new RestReporter(settings.ServerUri));
            }
        }

        if (settings.Strategy == "TCP_CONNECT")
        {
            detectStrategy = new TcpConnectDetectStrategy();
        }
    }

    public async Task Run()
    {
        // Print SharpPcap version
        string ver = SharpPcap.Version.VersionString;
        /* Print SharpPcap version */
        Console.WriteLine("SharpPcap {0}, Example6.DumpTCP.cs", ver);
        Console.WriteLine();

        /* Retrieve the device list */
        var devices = CaptureDeviceList.Instance;

        /*If no device exists, print error */
        if (devices.Count < 1)
        {
            Console.WriteLine("No device found on this machine");
            return;
        }

        Console.WriteLine("The following devices are available on this machine:");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine();

        int i = 0;

        /* Scan the list printing every entry */
        foreach (var dev in devices)
        {
            /* Description */
            Console.WriteLine("{0}) {1} {2}", i, dev.Name, dev.Description);
            i++;
        }

        Console.WriteLine();
        Console.Write("-- Please choose a device to capture: ");


        var device = devices[settings.IndexDevice];

        //Register our handler function to the 'packet arrival' event
        device.OnPacketArrival +=
            new PacketArrivalEventHandler(device_OnPacketArrival);

        // Open the device for capturing
        int readTimeoutMilliseconds = 1000;
        device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

        //tcpdump filter to capture only TCP/IP packets
        string filter = $"ip and tcp";
        device.Filter = filter;

        Console.WriteLine();
        Console.WriteLine
            ("-- The following tcpdump filter will be applied: \"{0}\"",
            filter);
        Console.WriteLine
            ("-- Listening on {0}, hit 'Ctrl-C' to exit...",
            device.Description);

        Console.CancelKeyPress += delegate
        {
            device.Close();
        };
        // Start capture 'INFINTE' number of packets
        try
        {
            while (true)
            {
                device.StartCapture();
                Thread.Sleep(TimeSpan.FromMinutes(settings.Duration));
                device.StopCapture();
                var supiciousSources = await detectStrategy.Scan(packets);
                foreach (var reporter in reporters)
                {
                    await reporter.Report(supiciousSources);
                }
                packets.Clear();
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            // Close the pcap device
            // (Note: this line will never be called since
            //  we're capturing infinite number of packets
            device.Close();
        }
    }

    /// <summary>
    /// Prints the time, length, src ip, src port, dst ip and dst port
    /// for each TCP/IP packet received on the network
    /// </summary>
    private void device_OnPacketArrival(object sender, CaptureEventArgs e)
    {
        var time = e.Packet.Timeval.Date;
        var len = e.Packet.Data.Length;

        var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

        var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
        if (tcpPacket != null)
        {
            var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
            if (!settings.IgnoreIps.Contains(ipPacket.SourceAddress.ToString()))
            {
                packets.Add(new TimeTcpPacket()
                {
                    Time = time,
                    TcpPacket = tcpPacket
                });
            }

        }
    }


    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
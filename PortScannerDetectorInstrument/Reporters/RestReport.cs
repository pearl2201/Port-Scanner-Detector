using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PortScannerDetectorInstrument.Entities;

namespace PortScannerDetectorInstrument.Reporters
{
    public class RestReporter : IReporter
    {
        private string _serverUri;
        public RestReporter(string serverUri)
        {
            _serverUri = serverUri;
            Console.WriteLine("[*] Server Uri: " + serverUri);
        }


        public async Task Report(List<SuspiciousSource> sources)
        {
            Console.WriteLine("[*] Source Length: " + sources.Count);
            if (sources.Count > 0)
            {
                try
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    //Console.WriteLine(JsonSerializer.Serialize(sources));
                    var message = new HttpRequestMessage(HttpMethod.Post, _serverUri)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(sources), Encoding.UTF8,
                                             "application/json"),
                    };
                    await client.SendAsync(message);

                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }
    }
}
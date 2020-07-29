using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortScannerDetectorServer.Data;
using Microsoft.EntityFrameworkCore;
using PortScannerDetectorServer.Entities;
using System.Text.Json;

namespace PortScannerDetectorServer.Services
{
    public class FcmService
    {
        private FirebaseApp defaultApp;
        public FcmService()
        {
            GoogleCredential googleCredential = GoogleCredential.FromFile("./alertportscanner-firebase-adminsdk-80rsu-110f68429b.json");
            if (FirebaseApp.DefaultInstance == null)
            {
                defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = googleCredential
                });
                Console.WriteLine("Create Firebaseapp");
            }

        }

        public async Task SendSuspiciousAddressesToMobileClient(List<Address> addresses)
        {
            Console.WriteLine("SendSuspiciousAddressesToMobileClient");
            FirebaseMessaging firebaseMessaging = FirebaseMessaging.GetMessaging(FirebaseApp.DefaultInstance);
            var response = await firebaseMessaging.SendAsync(new Message()
            {
                Topic = "Suspicious",
                Notification = new Notification()
                {
                    Title = "Suspicious",
                    Body = JsonSerializer.Serialize(addresses)
                }
            });
            Console.WriteLine(response);
        }
    }

}
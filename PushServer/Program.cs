using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Funq;
using PushServer.Model;
using PushSharp;
using PushSharp.Android;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.WindowsPhone;
using ServiceStack;

namespace PushServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:1338/" : args[0];
            
            var pushBroker = new PushBroker();
            pushBroker.OnNotificationSent += (sender, notification) =>
            {
                Console.Write("OK");
            };
            pushBroker.OnNotificationFailed += (sender, notification, error) =>
            {
                Console.Write(error.Message);
            };
            pushBroker.OnChannelException += (sender, channel, error) =>
            {
                Console.Write(error.Message);
            };

            var appHost = new Program.AppHost(pushBroker)
                .Init()
                .Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}",
                DateTime.Now, listeningOn);

            Console.ReadKey();
        }

        //Define the Web Services AppHost
        public class AppHost : AppSelfHostBase
        {
            private PushBroker _pushBroker;

            public AppHost(PushBroker pushBroker)
                : base("HttpListener Self-Host", typeof(PushService).Assembly)
            {
                _pushBroker = pushBroker;
            }

            public override void Configure(Funq.Container container)
            {
                _pushBroker.RegisterGcmService(new GcmPushChannelSettings(AppSettings.GetString("AndroidServerKey")));
            
                container.Register(c => _pushBroker).ReusedWithin(ReuseScope.Container);
                container.Register(new ServerSettings {ApiKey = AppSettings.GetString("ApiKey")});
            }
        }
    }
}

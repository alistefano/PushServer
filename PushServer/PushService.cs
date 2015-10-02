using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PushServer.Model;
using PushSharp;
using PushSharp.Android;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.WindowsPhone;
using ServiceStack;

namespace PushServer
{
    public class PushService : Service
    {
        public ServerSettings Settings { get; set; }
        public PushBroker PushBroker { get; set; }

        public object Any(PushRequest request)
        {
            //Check if the Api Key provided by client match the key in app.config
            if (request.ApiKey == Settings.ApiKey)
            {
                
                foreach (var pushNotification in request.Notifications)
                {
                    switch (pushNotification.Provider)
                    {
                        case Provider.Apple:
                            //NOTE: Apple Notification Payload has 256 chars limit.
                            PushBroker.QueueNotification(new AppleNotification().ForDeviceToken(pushNotification.PushId).WithCustomItem("data", HttpUtility.UrlDecode(pushNotification.Data)));
                            break;

                        case Provider.Android:
                            PushBroker.QueueNotification(new GcmNotification().ForDeviceRegistrationId(pushNotification.PushId)
                                .WithJson(HttpUtility.UrlDecode(pushNotification.Data)));
                            break;

                        case Provider.WindowsPhone:
                            PushBroker.QueueNotification(new WindowsPhoneRawNotification().ForEndpointUri(new Uri(pushNotification.PushId))
                                    .WithRaw(HttpUtility.UrlDecode(pushNotification.Data)));
                            break;
                    }
                }

                return new PushResponse() {Result = request.Notifications.Length + " notifications encoded"};
            }

            throw new HttpError(HttpStatusCode.Unauthorized, "Invalid Api Key");
        }
    }
}

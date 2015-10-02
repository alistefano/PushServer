using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace PushServer.Model
{
    [Route("/push")]
    public class PushRequest : IReturn<PushResponse>
    {
        public string ApiKey { get; set; }
        public PushNotification[] Notifications { get; set; }
    }

    public class PushResponse
    {
        public string Result { get; set; }
    }
}

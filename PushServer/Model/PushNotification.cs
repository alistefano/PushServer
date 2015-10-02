using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PushServer.Model
{
    public enum Provider { Apple, Android, WindowsPhone}
    
    public class PushNotification
    {
        public Provider Provider { get; set; }
        public string PushId { get; set; }
        public string Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LKDin.Messaging
{
    public class Message
    {
        public string Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Route { get; set; }

        public string Body { get; set; }
    }
}

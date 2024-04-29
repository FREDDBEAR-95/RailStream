using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    internal class Message
    {
        public bool status { get; }
        public string message { get; }

        public Message(bool sts, string msg) { 
            status = sts;
            message = msg;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.Other
{
    internal class ServerMessage
    {
        public bool status { get; }
        public string message { get; }

        public ServerMessage(bool sts, string msg)
        {
            status = sts;
            message = msg;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.Other
{
    public class ServerMessage
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public ServerMessage(bool status, string message) 
        {
            Status = status;
            Message = message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.Other
{
    public class ServerResponce
    {
        public bool Status { get; set; }

        public string? Content { get; set; }

        public ServerResponce(bool status, string content)
        {
            Status = status;
            Content = content;
        }
    }
}

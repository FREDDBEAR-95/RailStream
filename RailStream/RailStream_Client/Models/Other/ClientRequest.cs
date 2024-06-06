using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.Other
{
    public class ClientRequest
    {
        public string Headers { get; set; }

        public string? Content { get; set; }

        public ClientRequest(string headers, string content)
        {
            Headers = headers; 
            Content = content; 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.Other
{
    internal class ClientRequest
    {
        public Dictionary<string, string> Headers  = new Dictionary<string, string>();

        public string? Content;

        public ClientRequest(Dictionary<string, string> headers, string content) 
        { 
            Headers = headers;
            Content = content;
        }
    }
}

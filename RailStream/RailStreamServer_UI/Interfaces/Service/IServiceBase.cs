using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    internal interface IServiceBase
    {
        public string Name { get; }
        public string Description { get; }

        public void Start();
        public void Stop();
    }
}

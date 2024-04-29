using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    internal interface IOrderManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }
    }
}

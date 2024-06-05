using RailStream_Server.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    public interface ITicketManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }

        public ServerResponce CreateTicket(ClientRequest request);
        public ServerResponce ChangeTicket(ClientRequest request);
        public ServerResponce RemoveTicket(ClientRequest request);
    }
}

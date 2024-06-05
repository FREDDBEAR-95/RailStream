using RailStream_Server.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    public interface IUserManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }

        public ServerResponce LoginUser(ClientRequest request);

        public ServerResponce LogOutUser(ClientRequest request);

        public ServerResponce SingUpUser(ClientRequest request);
    }
}

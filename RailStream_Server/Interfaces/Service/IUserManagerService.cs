using RailStream_Server.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    internal interface IUserManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }

        void LoginUser(TcpClient client, ClientRequest request);

        public void LogOutUser(TcpClient client);

        public void SingUpUser(TcpClient client, ClientRequest request);
    }
}

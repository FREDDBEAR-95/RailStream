using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    public interface INotificationManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }

        public ServerResponce CreateNotification(ClientRequest clientRequest);
        public ServerResponce ChangeNotification(ClientRequest clientRequest);
        public ServerResponce RemoveNotification(ClientRequest clientRequest);
    }
}

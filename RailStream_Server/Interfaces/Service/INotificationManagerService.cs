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
    internal interface INotificationManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }

        public void AddNotification(Notification notification);
        public void ChangeNotification(Notification notification);
        public void RemoveNotification(Notification notification);

        public ServerResponce CheckNotification(ClientRequest clientRequest);
        public void MarkNotificationAsRead(ClientRequest clientRequest);
    }
}

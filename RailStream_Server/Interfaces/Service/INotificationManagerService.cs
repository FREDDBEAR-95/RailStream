using RailStream_Server.Models;
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

        void AddNotification(User user);
        void ChangeNotification(User user);
        void RemoveNotification(User user);
    }
}

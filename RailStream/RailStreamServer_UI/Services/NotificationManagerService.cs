using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    internal class NotificationManagerService : INotificationManagerService
    {
        public string Name { get; } = "NotificationManagerService";
        public string Description { get; } = "Notification Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}

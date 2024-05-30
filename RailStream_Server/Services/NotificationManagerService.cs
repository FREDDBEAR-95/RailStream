using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RailStream_Server.Models;
using RailStream_Server.Models.Other;

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

        // Функция добавления уведомления
        public void AddNotification(User user)
        {

        }

        // Функция изменения уведомления
        public void ChangeNotification(User user)
        {

        }

        // Функция удаления уведомления
        public void RemoveNotification(User user)
        {

        }
    }
}

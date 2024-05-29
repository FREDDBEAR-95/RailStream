using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server.Models.UserModel;

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

        // Метод возвращающая не прочитанные уведомления
        public ServerResponce CheckNotification(ClientRequest clientRequest)
        {
            return new ServerResponce(true, "");
        }

        // Метод отмечающий уведомление как прочитанное в БД.
        public void MarkNotificationAsRead(ClientRequest clientRequest)
        {

        }

        // Метод планирования отправки уведомления
        public void ScheduleNotification(Notification notification, DateTime time)
        {

        }

        // Метод получения истории уведомлений
        public ServerResponce GetNotificationHistory(ClientRequest clientRequest)
        {
            return new ServerResponce(true, "");
        }

        // Метод добавления уведомления
        public void AddNotification(Notification notification)
        {

        }

        // Метод изменения уведомления
        public void ChangeNotification(Notification notification)
        {

        }

        // Метод удаления уведомления
        public void RemoveNotification(Notification notification)
        {

        }
    }
}

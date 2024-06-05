using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server.Models.UserModel;
using RailStream_Server_Backend.Managers;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using Azure.Core;
using System.Net;
using Authorization = RailStream_Server.Models.Authorization;

namespace RailStream_Server.Services
{
    public class NotificationManagerService : INotificationManagerService
    {
        public string Name { get; } = "NotificationManagerService";
        public string Description { get; } = "Notification Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;

        public string configPath = @"Configs\\DatabaseConfig.json";

        #region ServiceControl
        // Метод запуска сервиса
        public void Start()
        {

        }

        // Метод остановки сервиса
        public void Stop()
        {

        }
        #endregion

        #region ServiceMethods
        // Метод возвращающая не прочитанные уведомления
        public ServerResponce CheckNotification(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                // Проверка тела запроса
                if (request.Content == null)
                {
                    serverResponse["Message"] = "Ошибка авторизации! Отсутствуют данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Десериализация тела запроса
                Dictionary<string, object>? data = JsonSerializer.Deserialize<Dictionary<string, object>>(request.Content);

                // Проверка полученных данных
                if (data == null)
                {
                    serverResponse["Message"] = "Ошибка авторизации! Не удалось получить данные из запроса.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
                
                // Получения списка уведомлений
                using (DatabaseManager dbManager = new DatabaseManager(configPath))
                {
                    Authorization? auth = dbManager.Authorizations.Where(auth => auth.SessionKey == (string)data["SessionKey"]).SingleOrDefault();
                    NotificationStatus? statusUnread = dbManager.NotificationStatus.Where(status => status.Status == "Не прочитано").SingleOrDefault();

                    if (auth == null)
                    {
                        serverResponse["Message"] = $"Не корректный ключ сессии";
                        return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                    }

                    if (statusUnread == null)
                    {
                        serverResponse["Message"] = $"Ошибка поиска статуса уведомления: Не прочитано";
                        return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                    }

                    try
                    {
                        NotificationStatus? statusScheduled = dbManager.NotificationStatus.Where(status => status.Status == "Запланировано").SingleOrDefault();
                        if (statusScheduled != null)
                        {
                            IEnumerable<Notification> notifications = dbManager.Notification.Where(notification => notification.NotificationStatusId == statusScheduled.NotificationStatusId).ToList();
                            foreach (Notification notification in notifications)
                            {
                                if (notification.CreationDateTime > DateTime.Now)
                                { 
                                    notification.NotificationStatusId = statusUnread.NotificationStatusId;
                                    ChangeNotification(notification);
                                }
                            }
                        }
                    }

                    catch (Exception e)
                    {

                    }

                    serverResponse["NotificationsList"] = dbManager.Notification.Where(notification => notification.UserId == auth.UserId && notification.NotificationStatusId == statusUnread.NotificationStatusId).ToList();
                }

                serverResponse["Message"] = "Список уведомлений";
                return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка авторизации! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        // Метод отмечающий уведомление как прочитанное в БД
        public void MarkNotificationAsRead(ClientRequest request)
        {
            try
            {
                // Проверка тела запроса
                if (request.Content != null)
                {
                    // Десериализация тела запроса
                    Notification? notification = JsonSerializer.Deserialize<Notification>(request.Content);

                    // Проверка полученных данных
                    if (notification != null)
                    {
                        ChangeNotification(notification);
                    }
                }
            }

            catch
            {

            }
        }

        // Метод планирования отправки уведомления
        public ServerResponce ScheduleNotification(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                // Проверка тела запроса
                if (request.Content == null)
                {
                    serverResponse["Message"] = "Ошибка авторизации! Отсутствуют данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Десериализация тела запроса
                Notification? notification = JsonSerializer.Deserialize<Notification>(request.Content);

                // Проверка полученных данных
                if (notification == null)
                {
                    serverResponse["Message"] = "Не корректный формат уведомления.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Регистрация уведомления
                CreateNotification(notification);

                serverResponse["Message"] = "Уведомление успешно запланировано.";
                return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка запланирования уведомления! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        #endregion

        #region NotificationControl

        // Метод добавления уведомления
        public ServerResponce CreateNotification(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                // Проверка тела запроса
                if (request.Content == null)
                {
                    serverResponse["Message"] = "Отсутствуют данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Десериализация тела запроса
                Notification? notification = JsonSerializer.Deserialize<Notification>(request.Content);

                // Проверка полученных данных
                if (notification == null)
                {
                    serverResponse["Message"] = "Не корректный формат уведомления.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                if (CreateNotification(notification))
                {
                    serverResponse["Message"] = "Уведомление успешно создано!";
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }

                else
                {
                    serverResponse["Message"] = "Не удалось создать уведомление!";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка создания уведомления! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        public bool CreateNotification(Notification notification) 
        {
            try
            {
                using (DatabaseManager databaseManager = new DatabaseManager(configPath))
                {
                    databaseManager.Add(notification);
                    databaseManager.SaveChanges();
                }

                return true;
            }

            catch 
            {
                return false;    
            }
        }


        // Метод изменения уведомления
        public ServerResponce ChangeNotification(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                // Проверка тела запроса
                if (request.Content == null)
                {
                    serverResponse["Message"] = "Отсутствуют данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Десериализация тела запроса
                Notification? notification = JsonSerializer.Deserialize<Notification>(request.Content);

                // Проверка полученных данных
                if (notification == null)
                {
                    serverResponse["Message"] = "Не корректный формат уведомления.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                if (ChangeNotification(notification))
                {
                    serverResponse["Message"] = "Уведомление успешно изменено!";
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }

                else
                {
                    serverResponse["Message"] = "Не удалось изменить уведомление!";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка изменения уведомления! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        public bool ChangeNotification(Notification notification)
        {
            try
            {
                using (DatabaseManager databaseManager = new DatabaseManager(configPath))
                {
                    databaseManager.Update(notification);
                    databaseManager.SaveChanges();
                }

                return true;
            }

            catch
            {
                return false;
            }
        }


        // Метод удаления уведомления
        public ServerResponce RemoveNotification(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                // Проверка тела запроса
                if (request.Content == null)
                {
                    serverResponse["Message"] = "Отсутствуют данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Десериализация тела запроса
                Notification? notification = JsonSerializer.Deserialize<Notification>(request.Content);

                // Проверка полученных данных
                if (notification == null)
                {
                    serverResponse["Message"] = "Не корректный формат уведомления.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                if (RemoveNotification(notification))
                {
                    serverResponse["Message"] = "Уведомление успешно удалено!";
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }

                else
                {
                    serverResponse["Message"] = "Не удалось удалить уведомление!";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка удаления уведомления! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        public bool RemoveNotification(Notification notification)
        {
            try
            {
                using (DatabaseManager databaseManager = new DatabaseManager(configPath))
                {
                    databaseManager.Remove(notification);
                    databaseManager.SaveChanges();
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        #endregion
    }
}

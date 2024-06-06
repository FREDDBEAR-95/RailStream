using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Interfaces.Service;
using RailStream_Server_Backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    public class RouteManagerService : IRouteManagerService
    {
        public string Name { get; } = "OrderManagerService";
        public string Description { get; } = "Order Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;
        public string configPath = @"Configs\\DatabaseConfig.json";

        #region ServiceControl

        public void Start()
        {
            Status = StatusService.Active;
        }

        public void Stop()
        {
            Status = StatusService.Inactive;
        }

        #endregion

        #region ServiceMethods

        public ServerResponce GetRoutes(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                using (DatabaseManager dbManager = new DatabaseManager(configPath))
                {
                    var routeStatus = dbManager.RouteStatus.Where(status => status.Status == "Активно").SingleOrDefault();

                    if (routeStatus != null)
                        serverResponse["RoutesList"] = dbManager.Routes.Where(route => route.RouteStatusId == routeStatus.RouteStatusId).ToList();
                }

                serverResponse["Message"] = "Список маршрутов.";
                return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
            }

            catch (Exception e)
            {
                serverResponse["Message"] = "Не удалось получить список маршрутов.";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        #endregion

        #region RouteControl

        // Метод добавления уведомления
        public ServerResponce CreateRoute(ClientRequest request)
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
                Route? Route = JsonSerializer.Deserialize<Route>(request.Content);

                // Проверка полученных данных
                if (Route == null)
                {
                    serverResponse["Message"] = "Не корректный формат маршрута.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                if (CreateRoute(Route))
                {
                    serverResponse["Message"] = "Маршрут успешно создано!";
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }

                else
                {
                    serverResponse["Message"] = "Не удалось создать маршрут!";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка создания маршрута! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        public bool CreateRoute(Route Route)
        {
            try
            {
                using (DatabaseManager databaseManager = new DatabaseManager(configPath))
                {
                    databaseManager.Add(Route);
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
        public ServerResponce ChangeRoute(ClientRequest request)
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
                Route? Route = JsonSerializer.Deserialize<Route>(request.Content);

                // Проверка полученных данных
                if (Route == null)
                {
                    serverResponse["Message"] = "Не корректный формат маршрута.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                if (ChangeRoute(Route))
                {
                    serverResponse["Message"] = "Маршрут успешно изменено!";
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }

                else
                {
                    serverResponse["Message"] = "Не удалось изменить маршрут!";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка изменения маршрута! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        public bool ChangeRoute(Route Route)
        {
            try
            {
                using (DatabaseManager databaseManager = new DatabaseManager(configPath))
                {
                    databaseManager.Update(Route);
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
        public ServerResponce RemoveRoute(ClientRequest request)
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
                Route? Route = JsonSerializer.Deserialize<Route>(request.Content);

                // Проверка полученных данных
                if (Route == null)
                {
                    serverResponse["Message"] = "Не корректный формат маршрута.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                if (RemoveRoute(Route))
                {
                    serverResponse["Message"] = "Маршрут успешно удалено!";
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }

                else
                {
                    serverResponse["Message"] = "Не удалось удалить маршрут!";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка удаления маршрута! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        public bool RemoveRoute(Route Route)
        {
            try
            {
                using (DatabaseManager databaseManager = new DatabaseManager(configPath))
                {
                    databaseManager.Remove(Route);
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

        public ServerResponce Command(string command, ClientRequest request)
        {
            switch (command)
            {
                default:
                    return new ServerResponce(false, "Не известная команда!");
            }
        }
    }
}

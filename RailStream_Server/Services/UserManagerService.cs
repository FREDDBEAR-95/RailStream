
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic.ApplicationServices;
using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Interfaces.Service;
using RailStream_Server_Backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Authorization = RailStream_Server.Models.Authorization;
using User = RailStream_Server.Models.User;
using BC = BCrypt.Net.BCrypt;

namespace RailStream_Server.Services
{
    public class UserManagerService : IUserManagerService
    {
        public string Name { get; } = "UserManagerService";
        public string Description { get; } = "User Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;

        public void Start()
        {
            Status = StatusService.Active;
        }

        public void Stop()
        {
            Status = StatusService.Inactive;
        }

        // Функция авторизации пользователя
        public ServerResponce LoginUser(ClientRequest request)
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

                // Десериализация JSON
                Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Content);

                // Проверка полученных данных
                if (data == null)
                {
                    serverResponse["Message"] = "Ошибка авторизации! Не удалось получить данные из запроса.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Получение пользователя из базы данных
                using (DatabaseManager databaseManager = new DatabaseManager(@"Configs\\DatabaseConfig.json"))
                {
                    User? user = databaseManager.Users.FirstOrDefault(u => u.Email == data["Login"] && u.Password == data["Password"]);

                    // Проверка пользователя
                    if (user == null)
                    {
                        serverResponse["Message"] = "Ошибка авторизации! Неправильный логин или пароль.";
                        return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                    }

                    var auth = new Authorization
                    {
                        SessionKey = Guid.NewGuid().ToString(),
                        Address = IPAddress.Parse(data["Address"]),
                        DeviceName = data["DeviceName"],
                        UserId = user.UserId,
                        AuthorizationDate = DateTime.Now,
                        ConnectionStatusId = 1
                    };

                    // Сохранение авторизации в базе данных
                    databaseManager.Authorizations.Add(auth);
                    databaseManager.SaveChanges();

                    serverResponse["Message"] = "Вы успешно авторизированы!";
                    serverResponse["SessionKey"] = auth.SessionKey;
                    return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
                }
            }
            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка авторизации! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        // Функция выхода пользователя из системы
        public ServerResponce LogOutUser(ClientRequest request) 
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

                // Десериализация JSON
                Dictionary<string, object>? data = JsonSerializer.Deserialize<Dictionary<string, object>>(request.Content);

                // Проверка полученных данных
                if (data == null)
                {
                    serverResponse["Message"] = "Ошибка выхода из системы! Некорректные данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                using (DatabaseManager databaseManager = new DatabaseManager(@"Configs\\DatabaseConfig.json"))
                {
                    // Поиск записи авторизации
                    Authorization? auth = databaseManager.Authorizations.Where(entity => entity.SessionKey == (string)data["SessionKey"]).FirstOrDefault();

                    // Проверка, что запись авторизации найдена
                    if (auth == null)
                    {
                        serverResponse["Message"] = "Ошибка выхода из системы! Неверный ключ сессии в запросе.";
                        return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                    }

                    // Удаление записи авторизации
                    databaseManager.Authorizations.Remove(auth);
                    databaseManager.SaveChanges();
                }

                serverResponse["Message"] = "Вы успешно вышли из системы!";
                return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
            }

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка выхода из системы! Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }

        // Функция регистрации пользователя
        public ServerResponce SingUpUser(ClientRequest request)
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try 
            {
                // Проверка, что тело запроса есть данные
                if (request.Content == null)
                {
                    serverResponse["Message"] = "Ошибка авторизации! Отсутствуют данные в запросе.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Десериализация объекта типа User
                User? user = JsonSerializer.Deserialize<User>(request.Content);
                

                // Проверка, что пользователь успешно создан
                if (user == null)
                {
                    serverResponse["Message"] = "Ошибка регистрации! Проверьте корректность введенных данных.";
                    return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
                }

                // Хеширование пароля пользователя
                user.Password = BC.HashPassword(user.Password);

                // Запись пользователя в БД
                using (DatabaseManager databaseManager = new DatabaseManager(@"Configs\\DatabaseConfig.json"))
                {
                    databaseManager.Users.Add(user);
                    databaseManager.SaveChanges();
                }

                serverResponse["Message"] = "Вы успешно зарегистрированы!";
                return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
            } 

            catch (Exception e)
            {
                serverResponse["Message"] = $"Ошибка регистрации! Проверьте корректность введенных данных или попробуйте зарегистрироваться позднее. Текст ошибки: {e.Message}";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
        }
    }
}

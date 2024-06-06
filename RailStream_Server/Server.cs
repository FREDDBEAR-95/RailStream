using Microsoft.VisualBasic.ApplicationServices;
using RailStream_Server.Models.Other;
using RailStream_Server.Services;
using RailStream_Server_Backend.Interfaces.Service;
using RailStream_Server_Backend.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;


namespace RailStream_Server
{
    public class Server
    {
        private TcpListener _server;

        public event EventHandler<TcpClient> ClientConnected; // Событие для нового подключения

        private IList<TcpClient> _clients = new List<TcpClient> { };

        public ServiceManager serviceManager = new ServiceManager(new List<IServiceBase> { });
        public string ServerStatus { get; private set; } = "Выключен";

        public Server(IPAddress address, ushort port) {
            _server = new TcpListener(IPAddress.Any, port);
            ClientConnected += HandleClient;
        }

        public Server(ushort port) { 
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            ClientConnected += HandleClient;
        }

        public ServerMessage Open()
        {
            if (ServerStatus == "Включен")
                return new ServerMessage(true, "The server is already up and running!");
            
            try
            {
                _server.Start();
                serviceManager.StartServices();
                ServerStatus = "Включен";
                Task.Run(() => WaitForConnections());
                return new ServerMessage(true, "Server successfully launched!");
            }

            catch (Exception ex)
            {
                return new ServerMessage(false, $"Failed to start the server! Error description: {ex.Message}");
            }
        }

        public ServerMessage Close() 
        {
            if (ServerStatus == "Выключен")
                return new ServerMessage(true, "The server has already stopped!");

            try
            {
                foreach (TcpClient client in _clients)
                {
                    client.Close();
                }
                ServerStatus = "Выключен";
                serviceManager.StopServices();
                _server.Stop();
                
                return new ServerMessage(true, "The server has been successfully stopped!");
            }

            catch (Exception ex)
            {
                return new ServerMessage(false, $"Failed to disconnect the server. Error description: {ex.Message}");
            }
        }

        private async void WaitForConnections()
        {
            while (ServerStatus == "Включен")
            {
                try
                {
                    // Ожидание нового подключения
                    var client = await _server.AcceptTcpClientAsync();

                    // Вызов события ClientConnected
                    ClientConnected?.Invoke(this, client);
                    _clients.Add(client);
                }

                catch (Exception ex) 
                { 
                
                }
            }
        }

        public void HandleClient(object sender, TcpClient client)
        {
            // Получение потока для чтения/записи
            NetworkStream stream = client.GetStream();

            ServerResponce response = new ServerResponce(false, $"Сервер: Не удалось обработать запрос.");

            // Чтение данных от клиента
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string req = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            ClientRequest? request = JsonSerializer.Deserialize<ClientRequest>(req);

            if (request != null)
            {
                Dictionary<string, string>? Headers = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Headers);

                if (Headers != null)
                {
                    try
                    {
                        var service = serviceManager.Services.Where(s => s.Name == Headers["ServiceName"]).FirstOrDefault();
                        if (service != null) 
                        {
                            response = service.Command(Headers["Command"], request);
                        }
                    }

                    catch (Exception ex)
                    {
                        response = new ServerResponce(false, $"Не удалось обработать запрос. Текст ошибки: {ex.Message}");
                    }
                }
            }

            // Отправка ответа клиенту
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
            stream.Write(responseBytes, 0, responseBytes.Length);

            // Закрытие подключения
            client.Close();
        }
    }
}
using Microsoft.VisualBasic.ApplicationServices;
using RailStream_Server.Models.Other;
using RailStream_Server.Services;
using RailStream_Server_Backend.Interfaces.Service;
using RailStream_Server_Backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace RailStream_Server
{
    public class Server
    {
        private TcpListener _server;

        public ServiceManager serviceManager = new ServiceManager(new List<IServiceBase> { });
        public string ServerStatus { get; private set; } = "Выключен";

        public Server(IPAddress address, ushort port) {
            _server = new TcpListener(IPAddress.Any, port);
        }

        public Server(ushort port) { 
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
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
                _server.Stop();
                serviceManager.StopServices();
                ServerStatus = "Выключен";
                return new ServerMessage(true, "The server has been successfully stopped!");
            }

            catch (Exception ex)
            {
                return new ServerMessage(false, $"Failed to disconnect the server. Error description: {ex.Message}");
            }
        } 
    }
}
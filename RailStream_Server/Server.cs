﻿using RailStream_Server.Models;
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
        public ServiceManager serviceManager = new ServiceManager(new List<IServiceBase> { new UserManagerService() });
        public bool ServerStatus { get; private set; } = false;

        public Server(IPAddress address, ushort port) {
            _server = new TcpListener(IPAddress.Any, port);
        }

        public Server(ushort port) { 
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        }

        public Message Open()
        {
            if (ServerStatus)
                return new Message(true, "The server is already up and running!");
            
            try
            {
                _server.Start();
                serviceManager.StartServices();
                ServerStatus = true;
                return new Message(true, "Server successfully launched!");
            }

            catch (Exception ex)
            {
                return new Message(false, $"Failed to start the server! Error description: {ex.Message}");
            }
        }

        public Message Close() 
        {
            if (!ServerStatus)
                return new Message(true, "The server has already stopped!");

            try
            {
                _server.Stop();
                serviceManager.StopServices();
                ServerStatus = false;
                return new Message(true, "The server has been successfully stopped!");
            }

            catch (Exception ex)
            {
                return new Message(false, $"Failed to disconnect the server. Error description: {ex.Message}");
            }
        }
    }
}
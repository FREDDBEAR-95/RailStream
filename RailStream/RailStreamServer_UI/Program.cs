﻿using RailStream_Server;
using RailStream_Server.Models;
using RailStream_Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RailStream_Server_Backend
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server(8080);
            Message msg = server.Open();

            var app = new App();
            app.Run();
        }
    }
}

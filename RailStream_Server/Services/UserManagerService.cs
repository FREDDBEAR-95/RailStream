
using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailStream_Server.Services
{
    internal class UserManagerService : IUserManagerService
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
        public void LoginUser(TcpClient client, ClientRequest request)
        {

        }

        // Функция выхода пользователя из системы
        public void LogOutUser(TcpClient client) 
        {
            
        }

        // Функция регистрации пользователя
        public void SingUpUser(TcpClient client, ClientRequest request)
        {

        }
    }
}

using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    public class TicketManagerService : ITicketManagerService
    {
        public string Name { get; } = "TicketManagerService";
        public string Description { get; } = "Ticket Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public ServerResponce GetTicket(ClientRequest request)
        {
            return new ServerResponce(true, "");
        }

        public ServerResponce GetTickets(ClientRequest request)
        {
            return new ServerResponce(true, "");
        }

        // Метод добавления маршрута
        public ServerResponce CreateTicket(ClientRequest clientRequest)
        {
            return new ServerResponce(true, "");
        }

        // Метод изменения маршрута
        public ServerResponce ChangeTicket(ClientRequest clientRequest)
        {
            return new ServerResponce(true, "");
        }

        // Метод удаления маршрута
        public ServerResponce RemoveTicket(ClientRequest clientRequest)
        {
            return new ServerResponce(true, "");
        }

    }
}

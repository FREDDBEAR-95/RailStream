using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    internal class TicketManagerService : ITicketManagerService
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
    }
}

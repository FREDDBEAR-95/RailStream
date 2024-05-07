using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    internal class RouteManagerService : IOrderManagerService
    {
        public string Name { get; } = "OrderManagerService";
        public string Description { get; } = "Order Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;

        public void Start()
        {
            Status = StatusService.Active;
        }

        public void Stop()
        {
            Status = StatusService.Inactive;
        }
    }
}

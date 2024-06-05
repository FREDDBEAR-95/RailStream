using RailStream_Server.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Interfaces.Service
{
    public interface IScheduleManagerService : IServiceBase
    {
        public StatusService Status { get; private protected set; }

        public ServerResponce CreateSchedule(ClientRequest request);
        public ServerResponce ChangeSchedule(ClientRequest request);
        public ServerResponce DeleteSchedule(ClientRequest request);
    }
}

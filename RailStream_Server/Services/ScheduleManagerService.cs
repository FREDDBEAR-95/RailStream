using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    public class ScheduleManagerService : IScheduleManagerService
    {
        public string Name { get; } = "ScheduleManagerService";
        public string Description { get; } = "Schedules Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public ServerResponce GetSchedule(ClientRequest request) 
        {
            return new ServerResponce(true, "");
        }

        
        
        public ServerResponce GetSchedules(ClientRequest request) 
        {
            return new ServerResponce(true, "");
        }


        
        
        public ServerResponce CreateSchedule(ClientRequest request) 
        {
            return new ServerResponce(true, "");
        }

        
        
        public ServerResponce ChangeSchedule(ClientRequest request) 
        {
            return new ServerResponce(true, "");
        }

        
        
        public ServerResponce DeleteSchedule(ClientRequest request) 
        {
            return new ServerResponce(true, "");
        }
    
    }
}

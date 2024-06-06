using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Interfaces.Service;
using RailStream_Server_Backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RailStream_Server.Services
{
    public class ScheduleManagerService : IScheduleManagerService
    {
        public string Name { get; } = "ScheduleManagerService";
        public string Description { get; } = "Schedules Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;
        public string configPath = @"Configs\\DatabaseConfig.json";

        public void Start()
        {

        }

        public void Stop()
        {

        }
        
        public ServerResponce GetSchedules(ClientRequest request) 
        {
            Dictionary<string, object> serverResponse = new Dictionary<string, object>();

            try
            {
                using (DatabaseManager dbManager = new DatabaseManager(configPath))
                {
                    var scheduleStatus = dbManager.RouteStatus.Where(status => status.Status == "Активно").SingleOrDefault();

                    if (scheduleStatus != null)
                        serverResponse["RoutesList"] = dbManager.Routes.Where(route => route.RouteStatusId == scheduleStatus.RouteStatusId).ToList();
                }

                serverResponse["Message"] = "Список расписания.";
                return new ServerResponce(true, JsonSerializer.Serialize(serverResponse));
            }

            catch (Exception e)
            {
                serverResponse["Message"] = "Не удалось получить список маршрутов.";
                return new ServerResponce(false, JsonSerializer.Serialize(serverResponse));
            }
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

        public ServerResponce Command(string command, ClientRequest request)
        {
            switch (command)
            {
                default:
                    return new ServerResponce(false, "Не известная команда!");
            }
        }
    }
}

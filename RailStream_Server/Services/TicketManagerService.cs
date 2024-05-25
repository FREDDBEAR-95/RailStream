using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server.Services.Filters;
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
    internal class TicketManagerService : ITicketManagerService
    {
        public string Name { get; } = "TicketManagerService";
        public string Description { get; } = "Ticket Management Service";
        public StatusService Status { get; set; } = StatusService.Inactive;
        public ClientRequestValidator Validator = new ClientRequestValidator();

        public void Start()
        {

        }

        public void Stop()
        {

        }

        // Функция добавления билета
        public ServerResponse RegisterTicket(ClientRequest clientRequest) 
        {
            Dictionary<string, string> serverResponce = new Dictionary<string, string>();

            try
            {
                Ticket? ticket = Validator.TicketRegisterValidate(clientRequest);
                if (ticket == null)
                {
                    serverResponce["Message"] = $"Ошибка покупки! Данные билета некорректны.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }
                
                using (DatabaseManager dbManager = new DatabaseManager())
                {
                    dbManager.Tickets.Add(ticket);
                    dbManager.SaveChanges();
                }
            } catch (Exception ex)
            {
                serverResponce["Message"] = $"Ошибка покупки! {ex.Message}, {ex.InnerException}";
                return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
            }

            serverResponce["Message"] = $"Вы успешно купили билет.";
            return new ServerResponse(true, JsonSerializer.Serialize(serverResponce));
        }

        // Функция удаления билета
        public ServerResponse RemoveTicket(ClientRequest clientRequest) 
        {
            Dictionary<string, string> serverResponce = new Dictionary<string, string>();

            try
            {
                int? ticketId = Validator.TicketRemoveValidate(clientRequest);
                if ( ticketId == null )
                {
                    serverResponce["Message"] = $"Ошибка возврата билета! Данные билета некорректные.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }

                using (DatabaseManager dbManager = new DatabaseManager())
                {
                    Ticket ticket = dbManager.Tickets.Find(ticketId);
                    dbManager.Tickets.Remove(ticket);
                    dbManager.SaveChanges();
                }

            } catch (Exception ex)
            {
                serverResponce["Message"] = $"Ошибка возврата билета! {ex.Message}";
                return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
            }

            serverResponce["Message"] = $"Вы успешно вернули билет.";
            return new ServerResponse(true, JsonSerializer.Serialize(serverResponce));
        }

        // Функция изменения билета
        public ServerResponse UpdateTicket(ClientRequest clientRequest) 
        {
            Dictionary<string, string> serverResponce = new Dictionary<string, string>();

            try
            {
                
                Ticket? ticket = Validator.TicketUpdateValidate(clientRequest);

                if (ticket == null)
                {
                    serverResponce["Message"] = "Ошибка изменения билета! Данные билета некорректны.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }

                using (DatabaseManager dbManager = new DatabaseManager())
                {
                    Ticket updatedTicket = dbManager.Tickets.Where(t => t.TicketId == ticket.TicketId).FirstOrDefault();
                    updatedTicket.PlaceNumber = ticket.PlaceNumber;
                    updatedTicket.TrainId = ticket.TrainId;
                    updatedTicket.RouteId = ticket.RouteId;

                    dbManager.Tickets.Update(updatedTicket);
                    dbManager.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                serverResponce["Message"] = $"Ошибка изменения билета! {ex.Message}";
                return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
            }

            serverResponce["Message"] = $"Вы успешно измененили билет.";
            return new ServerResponse(true, JsonSerializer.Serialize(serverResponce));
        }

    }
}

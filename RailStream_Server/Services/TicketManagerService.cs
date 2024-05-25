using Microsoft.EntityFrameworkCore;
using RailStream_Server.Models;
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

        // Функция добавления билета
        public ServerResponse RegisterTicket(ClientRequest clientRequest) 
        {
            Dictionary<string, string> serverResponce = new Dictionary<string, string>();

            try
            {
                if (clientRequest.Content == null)
                {
                    serverResponce["Message"] = "Ошибка покупки! Отсутствуют данные в запросе.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }

                List<string>? requestLines = JsonSerializer.Deserialize<List<string>>(clientRequest.Content);

                if (requestLines.Count >= 2)
                {
                    if (requestLines[0] == "" || requestLines[0] == null)
                    {
                        serverResponce["Message"] = "Ошибка покупки! Отсутствуют данные пользователя.";
                        return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                    }
                    if (requestLines[1] == "" || requestLines[1] == null)
                    {
                        serverResponce["Message"] = "Ошибка покупки! Отсутствуют данные билета.";
                        return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                    }

                    int id = JsonSerializer.Deserialize<int>(requestLines[0]);
                    Ticket? ticket = JsonSerializer.Deserialize<Ticket>(requestLines[1]);
                    ticket.UserId = id;

                    using (DatabaseManager dbManager = new DatabaseManager())
                    {
                        dbManager.Tickets.Add(ticket);
                        dbManager.SaveChanges();
                    }
                }
            } catch (Exception ex)
            {
                serverResponce["Message"] = $"Ошибка покупки! {ex.Message}";
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
                if (clientRequest.Content == null)
                {
                    serverResponce["Message"] = "Ошибка возврата билета! Отсутствуют данные в запросе.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }

                int ticketId = JsonSerializer.Deserialize<int>(clientRequest.Content);

                using (DatabaseManager dbManager = new DatabaseManager())
                {
                    Ticket? ticket = dbManager.Tickets.Find(ticketId);
                    if (ticket == null)
                    {
                        serverResponce["Message"] = $"Ошибка возврата билета! Данного билета не существует.";
                        return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                    }
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
                if (clientRequest.Content == null)
                {
                    serverResponce["Message"] = "Ошибка изменения билета! Отсутствуют данные в запросе.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }

                Ticket? ticket = JsonSerializer.Deserialize<Ticket>(clientRequest.Content);

                if (ticket == null)
                {
                    serverResponce["Message"] = "Ошибка изменения билета! Отсутствуют данные билета.";
                    return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                }

                using (DatabaseManager dbManager = new DatabaseManager())
                {
                    Ticket? oldTicket = dbManager.Tickets.Find(ticket.TicketId);

                    if (oldTicket == null)
                    {
                        serverResponce["Message"] = $"Ошибка изменения билета! Данного билета не существует.";
                        return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
                    }

                    dbManager.Tickets.Remove(oldTicket);
                    dbManager.Tickets.Add(ticket);
                    dbManager.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                serverResponce["Message"] = $"Ошибка возврата билета! {ex.Message}";
                return new ServerResponse(false, JsonSerializer.Serialize(serverResponce));
            }

            serverResponce["Message"] = $"Вы успешно вернули билет.";
            return new ServerResponse(true, JsonSerializer.Serialize(serverResponce));
        }

    }
}

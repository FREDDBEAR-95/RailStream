using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using RailStream_Server.Models;
using RailStream_Server.Models.Other;
using RailStream_Server_Backend.Managers;

namespace RailStream_Server.Services.Filters
{
    internal class ClientRequestValidator
    {
        public Ticket? TicketRegisterValidate(ClientRequest clientRequest)
        {
            Ticket? ticket = null;
            try
            {
                if (clientRequest.Content == "") return ticket;
                ticket = JsonSerializer.Deserialize<Ticket>(clientRequest.Content);
                if (ticket == null) return null;

                // ticket на null уже проверили, в нём уже должны быть данные
                using (DatabaseManager databaseManager = new DatabaseManager())
                {
                    // Проверяем наличие купленного билета на место в поезде и существует ли пользователь на которого мы хотим зарегестрировать билет
                    if (databaseManager.Tickets.Where(tik => tik.TrainId == ticket.TrainId && tik.PlaceNumber == ticket.PlaceNumber).Count() > 0 ||
                        databaseManager.Users.Where(user => user.UserId == ticket.UserId).Count() <= 0)
                        return null;
                }
                return ticket;
            } catch
            {
                return null;
            }
        }
        public int? TicketRemoveValidate(ClientRequest clientRequest)
        {
            int? ticketId = null;

            try
            {
                if (clientRequest.Content == "") return null;
                ticketId = JsonSerializer.Deserialize<int>(clientRequest.Content);
                if (ticketId == null) return null;

                using (DatabaseManager dbManager = new DatabaseManager())
                {
                    if (dbManager.Tickets.Find(ticketId) == null) return null;
                }
                return ticketId;
            } catch
            {
                return null;
            }
        }
        public Ticket? TicketUpdateValidate(ClientRequest clientRequest)
        {
            Ticket? ticket = null;
            try
            {
                if (clientRequest.Content == "") return ticket;
                ticket = JsonSerializer.Deserialize<Ticket>(clientRequest.Content);
                if (ticket == null) return null;

                // ticket на null уже проверили, в нём уже должны быть данные
                using (DatabaseManager databaseManager = new DatabaseManager())
                {
                    // проверяем существование билета, который хочем изменить
                    // существует ли билет с местом на которое мы хотим изменить
                    // существует ли пользователь на которого мы хотим изменить
                    if (databaseManager.Tickets.Where(t => t.TicketId == ticket.TicketId).Count() <= 0 ||
                        databaseManager.Tickets.Where(tik => tik.TrainId == ticket.TrainId && tik.PlaceNumber == ticket.PlaceNumber).Count() > 0)
                        return null;
                }
                return ticket;
            }
            catch
            {
                return null;
            }
        }
    }
}

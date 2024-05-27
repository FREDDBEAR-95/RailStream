using Microsoft.EntityFrameworkCore.Storage;
using RailStream_Server.Models;
using RailStream_Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RailStream_Server_Backend.Managers;
using System.IO;
using RailStream_Server.Models.Other;
using System.Text.Json;

namespace RailStream_Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //TicketManagerService ticketManagerService = new TicketManagerService();

            //using (var databaseManager = new DatabaseManager())
            //{
            //    if (databaseManager.Tickets.Count() == 0) Console.WriteLine($"Сисок билетов пуст");
            //    foreach ( var ticket in databaseManager.Tickets)
            //    {
            //        Console.WriteLine($"{ticket.TicketId}, {ticket.UserId}, {ticket.TrainId}, {ticket.PlaceNumber}");
            //    }


            //    Ticket newTicket = new Ticket() {
            //        TicketId = 2,
            //        UserId = 1,
            //        RouteId = 10,
            //        TrainId = 1,
            //        PlaceNumber = 2
            //    };
            //    //ClientRequest clientRequest = new ClientRequest(new Dictionary<string,string>(), JsonSerializer.Serialize(newTicket));
            //    //ServerResponse serverResponse = ticketManagerService.RegisterTicket(clientRequest);


            //    //ClientRequest clientRequest = new ClientRequest(new Dictionary<string, string>(), JsonSerializer.Serialize(1));
            //    //ServerResponse serverResponse = ticketManagerService.RemoveTicket(clientRequest);

            //    ClientRequest clientRequest = new ClientRequest(new Dictionary<string, string>(), JsonSerializer.Serialize(newTicket));
            //    ServerResponse serverResponse = ticketManagerService.UpdateTicket(clientRequest);

            //    Console.WriteLine(JsonSerializer.Deserialize<Dictionary<string, string>>(serverResponse.Content)["Message"]);

            //    if (databaseManager.Tickets.Count() == 0) Console.WriteLine($"Сисок билетов пуст");
            //    foreach (var ticket in databaseManager.Tickets)
            //    {
            //        Console.WriteLine($"{ticket.TicketId}, {ticket.UserId}, {ticket.TrainId}, {ticket.PlaceNumber}");
            //    }
            //}
        }
    }
}

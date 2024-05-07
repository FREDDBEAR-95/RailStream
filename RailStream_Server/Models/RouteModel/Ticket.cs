using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    internal class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }
        // ------------------------------------------------------
        public int UserId { get; set; } // внешний ключ
        public User? User { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int RouteId { get; set; }  // внешний ключ
        public Route? Route { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int TrainId { get; set; }  // внешний ключ
        public Train? Train { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int PlaceNumber { get; set; }
    }
}

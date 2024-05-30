using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    public class Route
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RouteId { get; set; }
        // ------------------------------------------------------
        public string RouteName { get; set; }
        // ------------------------------------------------------
        public int RouteStatusId { get; set; }        // внешний ключ
        public RouteStatus? RouteStatus { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public string DeparturePlace { get; set; }
        public string Destination { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public decimal Distance { get; set; }
        public string TimeWays { get; set; }
    }
}

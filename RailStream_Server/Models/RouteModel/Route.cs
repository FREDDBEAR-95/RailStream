using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    internal class Route
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
        public DateTime DepartureDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Distance { get; set; }
        public DateTime TimeWays { get; set; }

        public override string ToString()
        {
            return $"{RouteId}, {RouteName}, {RouteStatusId}, {DeparturePlace}, {Destination}, {DepartureDate.Date}, {DepartureTime.TimeOfDay}, {Distance}, {TimeWays.TimeOfDay}";
        }
    }
}

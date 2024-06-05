using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    public class Wagon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string WagonNumber { get; set; }
        // ------------------------------------------------------
        public int TrainId { get; set; }  // внешний ключ
        public Train? Train { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int WagonTypeId { get; set; }      // внешний ключ
        public WagonType? WagonType { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int? SeatsNumber { get; set; }
    }
}

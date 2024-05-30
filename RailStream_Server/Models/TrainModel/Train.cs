﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    public class Train
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainId { get; set; }
        // ------------------------------------------------------
        public int TrainTypeId { get; set; }      // внешний ключ
        public TrainType? TrainType { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int TrainStatusId { get; set; }        // внешний ключ
        public TrainStatus? TrainStatus { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public int RouteId { get; set; }  // внешний ключ (новое свойство)
        public Route? Route { get; set; } // навигационное свойство (новое свойство)
        // ------------------------------------------------------
        public string TrainBrand { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Location { get; set; }
    }
}

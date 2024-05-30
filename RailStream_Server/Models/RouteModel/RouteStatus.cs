using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    public class RouteStatus
    {
        [Key]
        public int RouteStatusId { get; set; }
        // ------------------------------------------------------
        public string Status { get; set; }
    }
}

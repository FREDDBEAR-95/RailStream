using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.UserModel
{
    public class ConnectionStatus
    {
        [Key]
        public int ConnectionStatusId { get; set; }
        // --------------------------------------------------
        public string Status { get; set; }
    }
}

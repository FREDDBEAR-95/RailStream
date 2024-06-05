using RailStream_Server.Models.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    public class Authorization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorizationId { get; set; }
        // --------------------------------------------------
        public string? SessionKey { get; set; }
        public IPAddress Address { get; set; }
        public string DeviceName { get; set; }
        // --------------------------------------------------
        public int UserId { get; set; } // внешний ключ
        public User? User { get; set; } // навигационное свойство
        // --------------------------------------------------
        public DateTime AuthorizationDate { get; set; }
        // --------------------------------------------------
        public int ConnectionStatusId { get; set; }             // внешний ключ
        public ConnectionStatus? ConnectionStatus { get; set; } // навигационное свойство
    }
}

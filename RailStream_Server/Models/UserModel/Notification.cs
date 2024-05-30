using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.UserModel
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        // --------------------------------------------------
        public int UserId { get; set; } // внешний ключ
        public User? User { get; set; } // навигационное свойство
        // --------------------------------------------------
        public string NotificationSubject { get; set; }
        public string NotificationText { get; set; }
        public DateTime CreationDateTime { get; set; }
        // --------------------------------------------------
        public int NotificationStatusId { get; set; }               // внешний ключ
        public NotificationStatus? NotificationStatus { get; set; } // навигационное свойство
    }
}

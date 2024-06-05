using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models.UserModel
{
    public class NotificationStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationStatusId { get; set; }
        // --------------------------------------------------
        public string Status { get; set; }
    }
}

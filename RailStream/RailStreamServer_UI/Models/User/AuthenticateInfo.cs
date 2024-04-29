using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server_Backend.Models.User
{

    internal class AuthenticateInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
        public IPAddress Address { get; set; }
        public ulong UserId { get; set; }
        public string UserName { get; set; }
        public DateTime AuthenticateDate { get; set; }
    }
}

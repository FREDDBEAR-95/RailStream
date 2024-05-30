using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        // --------------------------------------------------
        public string RoleTitle { get; set; }
    }
}

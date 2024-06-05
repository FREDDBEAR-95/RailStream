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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        // --------------------------------------------------
        public string RoleTitle { get; set; }
        public string? RoleDescription { get; set; }
        public IList<string> Sections { get; set; }
    }
}

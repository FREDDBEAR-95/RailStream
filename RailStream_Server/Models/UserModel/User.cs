using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailStream_Server.Models
{
    internal class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        // ------------------------------------------------------
        public int RoleId { get; set; } // внешний ключ
        public Role? Role { get; set; } // навигационное свойство
        // ------------------------------------------------------
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}

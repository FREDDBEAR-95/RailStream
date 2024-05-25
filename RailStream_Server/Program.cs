using Microsoft.EntityFrameworkCore.Storage;
using RailStream_Server.Models;
using RailStream_Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RailStream_Server_Backend.Managers;
using RailStream_Server.Services.DataGetter;
using System.IO;

namespace RailStream_Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (DatabaseManager databaseManager = new DatabaseManager())
            {
                Role newRole = new Role()
                {
                    RoleId = 1,
                    RoleDescription = "Пользователь",
                    RoleTitle = "",
                    Sections = new List<string>()
                };
            }
        }
    }
}

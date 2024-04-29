using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace RailStream_Server_Backend.Managers
{
    internal class DatabaseManager : DbContext
    {
        public static string GetDatabaseConfig(string path)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(path);
            var config = builder.Build();
            return config.GetConnectionString("DefaultConnection") ?? "";
        }


    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RailStream_Server.Models;
using RailStream_Server.Models.UserModel;


namespace RailStream_Server_Backend.Managers
{
    internal class DatabaseManager : DbContext
    {
        public string configPath = @"Configs\\DatabaseConfig.json";

        // Регистрация моделей
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Authorization> Authorizations { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Route> Routes { get; set; } = null!;
        public DbSet<RouteStatus> RouteStatus { get; set; } = null!;
        public DbSet<Train> Trains { get; set; } = null!;
        public DbSet<TrainStatus> TrainStatus { get; set; } = null!;
        public DbSet<TrainType> TrainType { get; set; } = null!;
        public DbSet<Wagon> Wagons { get; set; } = null!;
        public DbSet<WagonType> WagonType { get; set; } = null!;
        public DbSet<ConnectionStatus> ConnectionStatus { get; set; } = null!;
        public DbSet<Notification> Notification { get; set; } = null!;
        public DbSet<NotificationStatus> NotificationStatus { get; set; } = null!;

        public static IConfiguration GetDatabaseConfig(string path)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")));
            builder.AddJsonFile(path);
            var config = builder.Build();
            return config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetDatabaseConfig(configPath).GetConnectionString("DefaultConnection") ?? "");
        }
    }
}

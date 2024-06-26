﻿using AutoMapper;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Chat.Infra.Contexts
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.GetName().Name == "Chat");
            //var assembly = Assembly.LoadFrom(profilesPath);
            var typesToRegister = assembly.GetTypes()
            .Where(type => type.Name.EndsWith("Map") &&
                           type.GetInterfaces().Any(i => i.IsGenericType &&
                                                          i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<MessageRequest> MessageRequest { get; set; }

        public virtual DbSet<Message> Message { get; set; }

        public virtual DbSet<Conversation> Conversation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmailApi.Domain.Models;

namespace EmailApi.Persistence.Contexts
{
    public class AppDBContext : DbContext
    {
        public DbSet<Mail> Mails { get; set; }
        public AppDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Mail>().ToTable("Mails");
            builder.Entity<Mail>().HasKey(p => p.MailId);
            builder.Entity<Mail>().Property(p => p.MailId).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Mail>().Property(p => p.SenderAddress).IsRequired().HasMaxLength(50);
            //builder.Entity<Mail>().Property(p => p.State).IsRequired();
            builder.Entity<Mail>().Property(p => p.Subject).IsRequired().HasMaxLength(50);
            builder.Entity<Mail>().Property(p => p.Content).IsRequired();
            builder.Entity<Mail>().Property(p => p.Date).IsRequired().ValueGeneratedOnAdd();

        }
    }
}

